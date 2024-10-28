using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;





namespace NETCodeConsole
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var builder = new ConfigurationBuilder()
                 .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                 .Build();
            
            var services = new ServiceCollection();

            services.AddLogging(configure => configure.AddConsole());

            var serviceProvider = services.BuildServiceProvider();
            var logger = serviceProvider.GetRequiredService<ILogger<Service>>();


            Service service = new Service(logger, builder);
            service.FileMove();

            if (serviceProvider is IDisposable disposable)
            {
                disposable.Dispose();
            }
        }

        //private static void FileMove(ILogger<Program> logger)
        //{
        //   logger.LogInformation("This is an information log.");
        //   logger.LogWarning("This is a warning log.");
        //   logger.LogError("This is an error log.");


        //}
    }
}
