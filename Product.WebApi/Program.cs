using System;
using System.IO;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Product.WebApi.DataAccess;
using Microsoft.Extensions.Configuration;
using Serilog;
using Microsoft.Extensions.DependencyInjection;
using Product.WebApi.Data;

namespace Product.WebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                //Build Config
                var currentEnv = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
                var configuration = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json")
                    .AddJsonFile($"appsettings.{currentEnv}.json", optional: true)
                    .AddEnvironmentVariables()
                    .Build();

                Log.Logger = new LoggerConfiguration()
                    .ReadFrom.Configuration(configuration)
                    .CreateLogger();

                var host = CreateWebHostBuilder(args).Build();

                using (var scope = host.Services.CreateScope())
                {
                    var services = scope.ServiceProvider;
                    try
                    {
                        var context = services.GetRequiredService<ProductsContext>();
                        DbInitializer.Initialize(context);
                    }
                    catch (Exception ex)
                    {
                        var logger = services.GetRequiredService<ILogger>();
                        var errorMessage = "An error occurred initializing the database.";
                        logger.Error(ex, errorMessage);
                        Console.WriteLine($"{errorMessage}");
                    }
                }

                host.Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Host terminated unexpectedly");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseKestrel()
                .UseSerilog(dispose: true) // Set serilog as the loggin provider.
                .UseSerilog((context, config) =>
                {
                    config.ReadFrom.Configuration(context.Configuration); 
                })
                .UseStartup<Startup>()
                .UseDefaultServiceProvider(options =>
                    options.ValidateScopes = false)
                .UseIIS();

    }
}
