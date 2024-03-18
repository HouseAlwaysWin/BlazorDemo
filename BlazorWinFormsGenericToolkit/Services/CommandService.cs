using BlazorWinFormsGenericToolKit.Core.Extensions;
using BlazorWinFormsGenericToolKit.Models;
using CliWrap.EventStream;
using CliWrap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System.Text.Json;
using System.Diagnostics;
using BlazorWinFormsGenericToolKit.Shared.Dialogs;

namespace BlazorWinFormsGenericToolKit.Services
{
	public class CommandService
	{
		public DataTransferService<CommandGroup> _dataTransferService;
		public IDialogService _dialogService;
		public IConfigServices _confgService;
		public JsonNode Configs { get; set; }
		List<CommandGroup> CommandGroups;

		public CommandService(
			DataTransferService<CommandGroup> dataTransferService,
			IDialogService dialogService,
			IConfigServices confgService)
		{
			_dataTransferService = dataTransferService;
			_dialogService = dialogService;
			_confgService = confgService;

			Configs = _confgService.GetConfig();
		}

		/// <summary>
		/// 初始化ProcessID到config
		/// </summary>
		/// <returns></returns>
		public async Task GetSubProcessPID()
		{
			List<NetworkInfo> networkInfos = await ProcessTaskHelper.GetNetworkStates();
			var allGroups = await GetAllCommandGroups();
			if (allGroups != null)
			{
				foreach (var cmd in allGroups.SelectMany(a => a.CommandList).ToList())
				{
					var update = networkInfos.FirstOrDefault(n => n.LocalPort == cmd.Port);
					cmd.SubPID = update?.PID;
				}
			}
			SaveAllCommandGroup();
		}

		public async Task RunCommand(CommandItem cmd, CommandGroup currentCommandGroup)
		{
			try
			{
				cmd.IsRunning = true;
				List<NetworkInfo> networkInfos = await ProcessTaskHelper.GetNetworkStates();
				NetworkInfo networkInfo = networkInfos.FirstOrDefault(n => n.LocalPort == cmd.Port || n.PID == cmd.MainPID);
				if (networkInfo != null)
				{
					ProcessTaskHelper.KillProcess(networkInfo.PID);
				}

				Command cmdResult = Cli.Wrap(cmd.Command)
				   .WithArguments(cmd.Arguments)
				   .WithValidation(CommandResultValidation.None);

				Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
				IAsyncEnumerable<CommandEvent> cmdList = cmdResult.ListenAsync();
				if (!string.IsNullOrWhiteSpace(cmd.Encoding))
				{
					var encoding = Encoding.GetEncoding(cmd.Encoding);
					cmdList = cmdResult.ListenAsync(encoding);
				}

				await foreach (var cmdEvent in cmdList)
				{
					var newCmd = new JsonObject();

					switch (cmdEvent)
					{
						case StartedCommandEvent started:
							newCmd["Text"] = $"開始Process ID: {started.ProcessId}";
							newCmd["Type"] = "ProcessId";
							cmd.MainPID = started.ProcessId;
							SaveAllCommandGroup();
							break;
						case StandardOutputCommandEvent stdOut:
							newCmd["Text"] = stdOut.Text;
							newCmd["Type"] = "StdOut";
							break;
						case StandardErrorCommandEvent stdErr:
							newCmd["Text"] = stdErr.Text;
							newCmd["Type"] = "StdErr";
							break;
						case ExitedCommandEvent exited:
							newCmd["Text"] = $"已關閉Process, Code: {exited.ExitCode}";
							newCmd["Type"] = "Exited";
							break;
					}

					cmd.OutputLogs.Add(newCmd);
					await _dataTransferService.SendData(currentCommandGroup);
				}

			}
			catch (Exception ex)
			{
				throw ex;
			}
		}



		public async Task<List<CommandGroup>> GetAllCommandGroups()
		{
			if (CommandGroups == null)
			{
				CommandGroups = Configs.ToValue<List<CommandGroup>>("CommandGroups");
				if (CommandGroups == null || CommandGroups.Count == 0)
				{
					CommandGroups = CommandData.CommandGroups;
				}
			}
			return CommandGroups;
		}

		public async Task<CommandGroup> GetCommandGroup(string commandKey)
		{
			if (CommandGroups == null)
			{
				CommandGroups = Configs.ToValue<List<CommandGroup>>("CommandGroups");
				if (CommandGroups == null || CommandGroups.Count == 0)
				{
					CommandGroups = CommandData.CommandGroups;
				}
			}
			return CommandGroups?.FirstOrDefault(c => c.CommandKey == commandKey);
		}

		public void SaveAllCommandGroup()
		{
			var jsonStr = JsonSerializer.Serialize(CommandGroups);
			var jsonNode = JsonSerializer.Deserialize<JsonNode>(jsonStr);
			Configs["CommandGroups"] = jsonNode;
			_confgService.UpdateConfig(Configs);
		}

		public void CloseCmd(CommandItem item)
		{
			ProcessTaskHelper.KillProcess(item.MainPID);
			item.MainPID = null;
			item.IsRunning = false;
			SaveAllCommandGroup();
		}

		public void CloaseCmdGroup(CommandGroup group)
		{
			foreach (var item in group.CommandList)
			{
				CloseCmd(item);
			}
		}

		public async Task CloseAllProcess()
		{
			try
			{
				var allCmdList = CommandGroups.SelectMany(c => c.CommandList).ToList();
				foreach (var cmd in allCmdList)
				{
					ProcessTaskHelper.KillProcess(cmd.MainPID);
					//await IPInfoHelper.KillProcessFromPort(cmd.Port);
					cmd.MainPID = null;
					cmd.IsRunning = false;
				}
				SaveAllCommandGroup();
			}
			catch (Exception ex)
			{

				await _dialogService.ShowMessageBox("發生錯誤", ex.ToString());
			}

		}

		public async Task<bool> RunGitCmd(string cmd, string projectRoot, List<string> msgBuilder)
		{

			var err = new StringBuilder();
			Command cmdResult = Cli.Wrap("git")
					   .WithWorkingDirectory(projectRoot)
					   .WithArguments(cmd)
					   .WithValidation(CommandResultValidation.None);
			msgBuilder.Add($"git {cmd}");


			await foreach (var cmdEvent in cmdResult.ListenAsync())
			{
				var newCmd = new JsonObject();

				switch (cmdEvent)
				{
					case StandardOutputCommandEvent stdOut:
						msgBuilder.Add(stdOut.ToString());
						return true;
					case StandardErrorCommandEvent stdErr:
						msgBuilder.Add(stdErr.Text.ToString());
						return false;
				}

			}
			return true;
		}

		//public async Task SaveAllCommandGroups(List<CommandGroup> commandGroups)
		//{
		//	CommandGroups = commandGroups;
		//	var jsonStr = JsonSerializer.Serialize(commandGroups);
		//	var jsonNode = JsonSerializer.Deserialize<JsonNode>(jsonStr);
		//	Configs["CommandGroups"] = jsonNode;
		//	_confgService.UpdateConfig(Configs);
		//}

		//public async Task SaveCommandGroup(CommandGroup group)
		//{
		//	var commandGroup = CommandGroups.FirstOrDefault(c => c.CommandKey == group.CommandKey);
		//	commandGroup.GroupName = group.GroupName;
		//	commandGroup.CommandName = group.CommandName;
		//	commandGroup.CommandList = group.CommandList;
		//	var jsonStr = JsonSerializer.Serialize(CommandGroups);
		//	var jsonNode = JsonSerializer.Deserialize<JsonNode>(jsonStr);
		//	Configs["CommandGroups"] = jsonNode;
		//	_confgService.UpdateConfig(Configs);
		//}
	}
}
