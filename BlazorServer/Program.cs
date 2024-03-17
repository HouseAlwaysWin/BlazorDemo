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

            // �]�wRazorPage
            builder.Services.AddRazorPages();
            // �]�wBlazor Server
            builder.Services.AddServerSideBlazor();
            // �̪ۨ`�J�A��
            builder.Services.AddSingleton<WeatherForecastService>();

            var app = builder.Build();

            if (!app.Environment.IsDevelopment())
            {
                // �o�Ϳ��~���ɭԭ��s�ɦV�쭶�� Error
                app.UseExceptionHandler("/Error");
                // �ΨӱN HTTP �Y�檺�ǿ�w���ʳq�T��w (HSTS) ���Y�ǰe���Τ�ݪ� HSTS �����n��
                app.UseHsts();
            }
            // �ΨӱN HTTP �n�D���s�ɦV�� HTTPS �� HTTPS ���s�ɦV�����n��
            app.UseHttpsRedirection();

            // �����R�A�ɮ׳]�w,�w�]�ؿ��� �q�`���bwwwroot
            app.UseStaticFiles();

            // �]�O����
            app.UseRouting();

            // �]�w�P�s�����Y�ɳs�u�����I
            app.MapBlazorHub();
            // �]�w���ε{�� (Pages/_Host.cshtml) ���ڭ���
            app.MapFallbackToPage("/_Host");

            // �������ε{��
            app.Run();
        }
    }
}