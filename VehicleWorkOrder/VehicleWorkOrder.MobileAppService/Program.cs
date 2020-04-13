namespace VehicleWorkOrder.MobileAppService
{
    using System;
    using System.IO;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Azure.KeyVault;
    using Microsoft.Azure.Services.AppAuthentication;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Configuration.AzureKeyVault;
    using Microsoft.Extensions.Hosting;
    using Serilog;
    using Serilog.Events;

    public class Program
    {
        public static IConfiguration Configuration { get; } = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            //.AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}")
            .Build();

        public static async Task Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration().ReadFrom.Configuration(Configuration)
                .Enrich.FromLogContext().WriteTo
                .ColoredConsole(LogEventLevel.Verbose,
                    "{NewLine}{Timestamp:HH:mm:ss} [{Level}] ({CorrelationToken}) {Message}{NewLine}{Exception}")
                .CreateLogger();
            try
            {
                Log.Information("Host starting...");
                var host = CreateWebHostBuilder(args).Build(); //.Run();
                host.Run();
            }
            catch (Exception e)
            {
                Log.Fatal(e, "Host terminated unexpectedly.");
            }
            finally
            {
                Log.CloseAndFlush();
            }
            
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .UseConfiguration(Configuration)
                .ConfigureAppConfiguration((context, config) =>
                {
                    //if (context.HostingEnvironment.IsProduction())
                    //{
                        var builtConfig = config.Build();

                        var azureServiceTokenProvider = new AzureServiceTokenProvider();
                        var keyVaultClient = new KeyVaultClient(
                            new KeyVaultClient.AuthenticationCallback(
                                azureServiceTokenProvider.KeyVaultTokenCallback));

                        config.AddAzureKeyVault(
                            $"https://{builtConfig["KeyVaultName"]}.vault.azure.net/",
                            keyVaultClient,
                            new DefaultKeyVaultSecretManager());
                    //}
                })
                .UseSerilog((hostingContext, loggerConfiguration) =>
                {
                    loggerConfiguration.ReadFrom.Configuration(Configuration)
                    .WriteTo.ApplicationInsights(hostingContext.Configuration.GetSection("ApplicationInsights:InstrumentationKey").Value, TelemetryConverter.Events)
                    .WriteTo.MSSqlServer(hostingContext.Configuration["ConnectionStrings:WorkOrderConnection"], "LogTable", autoCreateSqlTable: true)
                    .Enrich.FromLogContext().WriteTo
                    .ColoredConsole(LogEventLevel.Verbose,
                        "{NewLine}{Timestamp:HH:mm:ss} [{Level}] ({CorrelationToken}) {Message}{NewLine}{Exception}");
                });

    }
}