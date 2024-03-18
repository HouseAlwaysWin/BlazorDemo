using Microsoft.AspNetCore.Builder;

namespace BlazorWinFormsGenericToolKit
{
	internal static class Program
	{
		/// <summary>
		///  The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main()
		{
			Startup.Init();
			ApplicationConfiguration.Initialize();
			Application.Run(new Form1());
		}
	}
}