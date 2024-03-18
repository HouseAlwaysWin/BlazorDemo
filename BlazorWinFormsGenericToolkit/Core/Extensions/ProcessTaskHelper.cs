using BlazorWinFormsGenericToolKit.Models;
using CliWrap;
using CliWrap.Buffered;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BlazorWinFormsGenericToolKit.Core.Extensions
{
	public class ProcessTaskHelper
	{

		/// <summary>
		/// 取得所有IP資訊
		/// </summary>
		/// <returns></returns>
		public static async Task<List<NetworkInfo>> GetNetworkStates()
		{
			var cmd = Cli.Wrap("netstat").WithArguments("-ano").WithValidation(CommandResultValidation.None);
			var result = await cmd.ExecuteBufferedAsync();
			List<NetworkInfo> networkInfos = new List<NetworkInfo>();
			if (result.ExitCode == 0)
			{
				string netstatOutput = result.StandardOutput;

				// 使用正则表达式将 netstat 输出解析为对象列表
				networkInfos = ParseNetworkInfo(netstatOutput);
			}
			return networkInfos;
		}

		/// <summary>
		/// 解析訊息
		/// </summary>
		/// <param name="inputText"></param>
		/// <returns></returns>
		private static List<NetworkInfo> ParseNetworkInfo(string inputText)
		{
			List<NetworkInfo> networkInfos = new List<NetworkInfo>();

			// 使用正则表达式匹配每一行的数据
			string pattern = @"(TCP|UDP)\s+(\S+)\s+(\S+)\s+(\S+)\s+(\d+)";
			foreach (Match match in Regex.Matches(inputText, pattern))
			{
				string portPattern = @"";
				var localAddress = match.Groups[2].Value;
				var localPort = ExtractPort(localAddress);
				var externalAddress = match.Groups[3].Value;
				var externalPort = ExtractPort(externalAddress);

				NetworkInfo info = new NetworkInfo
				{
					Protocol = match.Groups[1].Value,
					LocalAddress = localAddress,
					LocalPort = localPort,
					ExternalAddress = externalAddress,
					ExternalPort = externalPort,
					Status = match.Groups[4].Value,
					PID = int.Parse(match.Groups[5].Value)
				};
				networkInfos.Add(info);
			}
			return networkInfos;
		}

		/// <summary>
		/// 取出Port
		/// </summary>
		/// <param name="address"></param>
		/// <returns></returns>
		private static int ExtractPort(string address)
		{
			// 从地址字符串中提取端口号
			string[] parts = address.Split(new char[] { ':', ']' }, StringSplitOptions.RemoveEmptyEntries);
			if (parts.Length >= 1 && int.TryParse(parts[parts.Length - 1], out int port))
			{
				return port;
			}
			return -1; // 无法提取端口号

		}

		/// <summary>
		/// 判斷Process是否存在
		/// </summary>
		/// <param name="processId"></param>
		/// <returns></returns>
		public static bool IsProcessExisted(int? processId, out Process process)
		{

			process = Process.GetProcesses().FirstOrDefault(p => p.Id == processId);
			if (process != null && processId != 0)
			{
				return true;
			}
			return false;
		}

		public static async Task KillProcessFromPort(int? port)
		{
			var ipState = await GetNetworkStates();
			var process = ipState.FirstOrDefault(i => i.LocalPort == port);
			KillProcess(process?.PID);
		}

		public static void KillProcess(int? processId)
		{
			try
			{
				if (IsProcessExisted(processId, out Process process))
				{
					process.CloseMainWindow();
					process.Kill();
				}
			}
			catch (Exception ex)
			{
				throw new Exception($"Failed to terminate process with PID {processId}: {ex.Message}");
			}
		}
	}

}
