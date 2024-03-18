using BlazorWinFormsGenericToolKit.Core.Extensions;
using BlazorWinFormsGenericToolKit.Models;
using BlazorWinFormsGenericToolKit.Services;
using BlazorWinFormsGenericToolKit.Shared.Dialogs;
using Microsoft.AspNetCore.Components.WebView.WindowsForms;
using Microsoft.Extensions.DependencyInjection;
using MudBlazor;
using MudBlazor.Services;
using System;
using System.Reflection;

namespace BlazorWinFormsGenericToolKit
{
	public partial class Form1 : Form
	{

		public Form1()
		{
			InitializeComponent();
			DisplayVersionInTitle();


			this.MinimumSize = new System.Drawing.Size(1250, 600);
			var blazor = new BlazorWebView()
			{
				Dock = DockStyle.Fill,
				HostPage = "wwwroot/index.html",
				Services = Startup.Services!
			};

			blazor.RootComponents.Add<Main>("#app");
			Controls.Add(blazor);
			this.Icon = new Icon("Resources/Images/title.ico");
			FormClosed += Form1_FormClosed;
		}

		private async void Form1_FormClosed(object? sender, FormClosedEventArgs e)
		{
			await CloseAllProcess();
		}

		public async Task CloseAllProcess()
		{
			// 创建依赖注入容器
			var serviceProvider = Startup.Services;
			var commandService = serviceProvider.GetRequiredService<CommandService>();
			await commandService.CloseAllProcess();
		}

		private void DisplayVersionInTitle()
		{
			// 获取当前程序集的版本信息
			Version version = Assembly.GetEntryAssembly()?.GetName().Version;

			if (version != null)
			{
				string versionString = $"{version.Major}.{version.Minor}.{version.Build}.{version.Revision}";
				Text = $"泛用工具 v{versionString}";
			}
		}

	}
}