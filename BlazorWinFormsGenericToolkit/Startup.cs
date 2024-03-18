using BlazorWinFormsGenericToolKit.Data;
using BlazorWinFormsGenericToolKit.Repositories;
using BlazorWinFormsGenericToolKit.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Razor.RuntimeCompilation;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using MudBlazor.Services;
using System;

namespace BlazorWinFormsGenericToolKit
{
	public static class Startup
	{
		public static IServiceProvider? Services { get; private set; }

		public static void Init()
		{
			//var host = Host.CreateDefaultBuilder()
			//			   .ConfigureServices(WireupServices)
			//			   .Build();

			var builder = WebApplication.CreateBuilder();
			WireupServices(builder.Services);
			var app = builder.Build();
			Services = app.Services;
		}



		private static void WireupServices(IServiceCollection services)
		{
			services.AddWindowsFormsBlazorWebView();
			services.AddSingleton<WeatherForecastService>();
			services.AddTransient<IFolderPicker, FolderPicker>();
			services.AddSingleton<IConfigServices, ConfigServices>();
			services.AddSingleton(typeof(DataTransferService<>));
			services.AddSingleton<CommandService>();
			services.AddMudServices();
			services.BuildServiceProvider();
			services.AddLogging();
			services.AddSignalR();

			var rootPath = @$"{AppDomain.CurrentDomain.BaseDirectory}Core\Templates";
			services.AddMvcCore().AddRazorRuntimeCompilation(opts =>
			{
				opts.FileProviders.Add(new PhysicalFileProvider(rootPath)); // This will be the root path
			});


			services.AddRazorTemplating();


#if DEBUG
			services.AddBlazorWebViewDeveloperTools();
#endif
		}

	}
}