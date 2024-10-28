using DemoCoreMVC.Models;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace DemoCoreMVC
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();            
            
            builder.Services.AddDbContext<AdventureWorksLT2022Context>(
                  options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            var configurationBuilder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .Build();

            string? logPath = configurationBuilder.GetSection("LogPath").Value;

            // 設定 Serilog
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information() 
                .WriteTo.File($"{logPath}/log.txt", rollingInterval: RollingInterval.Day)
                .CreateLogger();

            Log.Information("HW02：維護(產品)網站");
            Log.Information($"logPath：{logPath}");

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Products}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
