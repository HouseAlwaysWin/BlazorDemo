using BlazorWinFormsGenericToolKit.Core.Extensions;
using BlazorWinFormsGenericToolKit.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace BlazorWinFormsGenericToolKit.Models
{
	public class CommandGroup
	{
		public string GroupName { get; set; }
		public string CommandKey { get; set; } = Guid.NewGuid().ToString();
		public string CommandName { get; set; }
		public List<CommandItem> CommandList { get; set; } = new List<CommandItem>();
		public bool IsRunning
		{
			get
			{
				return CommandList.Any(c => c.IsRunning || c.MainPID != null);
			}
		}
	}

	public static class CommandData
	{
		public static List<CommandGroup> CommandGroups = new List<CommandGroup>()
		{
			new CommandGroup {
			  GroupName = "系統登入",
			  CommandName= "後端登入",
			  CommandList=new List<CommandItem> {
				new CommandItem{ Command="dotnet", Arguments ="run --project D:\\ShittyProject\\dlp-develop\\DLP.WebAPI\\DLP.WebAPI.SignalR\\DLP.WebAPI.SignalR.csproj",Port = 31102 },
				new CommandItem{ Command="dotnet", Arguments ="run --project D:\\ShittyProject\\dlp-develop\\DLP.WebAPI\\DLP.WebAPI.AppPortal\\DLP.WebAPI.AppPortal.csproj",Port = 31101 }
			  }
			},
			new CommandGroup
			{
				GroupName = "系統模組",
				CommandName = "MD模組",
				CommandList = new List<CommandItem> {
				new CommandItem { Command = "dotnet", Arguments ="run --project D:\\ShittyProject\\dlp-develop\\DLP.WebAPI\\DLP.WebAPI.MD\\DLP.WebAPI.MD.csproj",Port=31108 },

			  }
			},

			new CommandGroup
			{
				GroupName = "系統模組",
				CommandName = "MG模組",
				CommandList = new List<CommandItem> {
				new CommandItem { Command = "dotnet", Arguments ="run --project D:\\ShittyProject\\dlp-develop\\DLP.WebAPI\\DLP.WebAPI.MG\\DLP.WebAPI.MG.csproj",Port=31109 },
			  }
			},
			new CommandGroup
			{
				GroupName = "系統模組",
				CommandName = "MP模組",
				CommandList = new List<CommandItem> {
				new CommandItem { Command="dotnet", Arguments="run --project D:\\ShittyProject\\dlp-develop\\DLP.WebAPI\\DLP.WebAPI.MP\\DLP.WebAPI.MP.csproj", Port = 31112 },

			  }
			},
			new CommandGroup
			{
				GroupName = "系統模組",
				CommandName = "MI模組",
				CommandList = new List<CommandItem> {
				new CommandItem { Command ="dotnet", Arguments="run --project D:\\ShittyProject\\dlp-develop\\DLP.WebAPI\\DLP.WebAPI.MI\\DLP.WebAPI.MI.csproj", Port =31110},

			  }
			},
			new CommandGroup
			{
				GroupName = "系統模組",
				CommandName = "SD模組",
				CommandList = new List<CommandItem> {
				new CommandItem { Command = "dotnet",Arguments="run --project D:\\ShittyProject\\dlp-develop\\DLP.WebAPI\\DLP.WebAPI.SD\\DLP.WebAPI.SD.csproj", Port=31107 },

			  }
			},
			new CommandGroup
			{
				GroupName = "系統模組",
				CommandName = "PM模組",
				CommandList = new List<CommandItem> {
				new CommandItem { Command = "dotnet",Arguments="run --project D:\\ShittyProject\\dlp-develop\\DLP.WebAPI\\DLP.WebAPI.PM\\DLP.WebAPI.PM.csproj", Port =31113},

			  }
			},
			new CommandGroup
			{
				GroupName = "系統模組",
				CommandName = "WO模組",
				CommandList = new List<CommandItem> {
				new CommandItem { Command = "dotnet",Arguments="run --project D:\\ShittyProject\\dlp-develop\\DLP.WebAPI\\DLP.WebAPI.WO\\DLP.WebAPI.WO.csproj",Port=31105 },

			  }
			},
			new CommandGroup
			{
				GroupName = "前端",
				CommandName = "前端啟動HostBuild",
				CommandList = new List<CommandItem> {
				new CommandItem { Command = "cmd",Arguments="/c D:\\Tools\\MaxGodTool\\run-frontend-buildhost.bat",Port = 31000, Encoding ="big5"  },
			  }
			},
			new CommandGroup
			{
				GroupName = "前端",
				CommandName = "前端啟動",
				CommandList = new List<CommandItem> {
					new CommandItem { Command = "cmd",Arguments="/c D:\\Tools\\MaxGodTool\\run-frontend.bat",Port = 31000,Encoding ="big5" }
			  }
			},
			new CommandGroup
			{
				GroupName = "前端",
				CommandName = "前端取版重建",
				CommandList = new List<CommandItem> {
					new CommandItem { Command = "cmd",Arguments="/c D:\\Tools\\MaxGodTool\\rebuild.bat" , Port = 31000, Encoding = "big5"}
			  }
			},
			new CommandGroup
			{
				GroupName = "舊系統",
				CommandName = "舊系統(VN)",
				CommandList = new List<CommandItem> {
				new CommandItem { Command="cmd", Arguments="/c start iexplore http://172.32.1.13:8888/forms/frmservlet?config=db19c & exit" }
			  }
			},
			new CommandGroup
			{
				GroupName = "舊系統",
				CommandName = "舊系統(VS)",
				CommandList = new List<CommandItem> {
				new CommandItem { Command="cmd", Arguments="/c javaws.exe D:\\Tools\\MaxGodTool\\publish\\ExecuteFile\\VSDB2.jnlp & exit"}
			  }
			},
			new CommandGroup
			{
				GroupName = "舊系統",
				CommandName = "舊系統(DG)",
				CommandList = new List<CommandItem> {
				new CommandItem { Command="cmd", Arguments="/c start iexplore http://172.29.1.12:8888/forms/frmservlet?config=dgdbt & exit"}
			  }
			},
			new CommandGroup
			{
				GroupName = "舊系統",
				CommandName = "舊系統(TC)",
				CommandList = new List<CommandItem> {
				new CommandItem { Command="cmd", Arguments="/c javaws.exe D:\\Tools\\MaxGodTool\\publish\\ExecuteFile\\TCFFD.jnlp & exit"}
			  }
			},
			new CommandGroup
			{
				GroupName = "發版",
				CommandName = "發版工具",
				CommandList = new List<CommandItem> {
				new CommandItem { Command="D:\\Tools\\MaxGodTool\\deployScript\\DeployCmd.exe", Arguments=""}
			  }
			},
		};
	}
}
