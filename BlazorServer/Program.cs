using BlazorServer.Data;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace BlazorServer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // 設定RazorPage
            builder.Services.AddRazorPages();
            // 設定Blazor Server
            builder.Services.AddServerSideBlazor();
            // 相依注入服務
            builder.Services.AddSingleton<WeatherForecastService>();

            var app = builder.Build();

            if (!app.Environment.IsDevelopment())
            {
                // 發生錯誤的時候重新導向到頁面 Error
                app.UseExceptionHandler("/Error");
                // 用來將 HTTP 嚴格的傳輸安全性通訊協定 (HSTS) 標頭傳送給用戶端的 HSTS 中介軟體
                app.UseHsts();
            }
            // 用來將 HTTP 要求重新導向至 HTTPS 的 HTTPS 重新導向中介軟體
            app.UseHttpsRedirection();

            // 提供靜態檔案設定,預設目錄為 通常都在wwwroot
            app.UseStaticFiles();

            // 設令路由
            app.UseRouting();

            // 設定與瀏覽器即時連線的端點
            app.MapBlazorHub();
            // 設定應用程式 (Pages/_Host.cshtml) 的根頁面
            app.MapFallbackToPage("/_Host");

            // 執行應用程式
            app.Run();
        }
    }
}