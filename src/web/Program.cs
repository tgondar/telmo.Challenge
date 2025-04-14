using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Raven.Client.Documents;
using Raven.Client.Documents.Session;
using Serilog;
using System;

namespace Backend.Challenge
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(new ConfigurationBuilder()
                    .AddJsonFile("appsettings.json")
                    .Build())
                .CreateLogger();

            try
            {
                Log.Information("Starting up the application");
                CreateHostBuilder(args).Build().Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Application start-up failed");
                throw;
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseSerilog()
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                })
                .ConfigureServices((context, services) =>
                {
                    // Configure RavenDB Document Store
                    var ravenDbConfig = context.Configuration.GetSection("RavenDB");
                    var urls = ravenDbConfig.GetSection("Urls").Get<string[]>();
                    var database = ravenDbConfig["Database"];

                    var store = new DocumentStore
                    {
                        Urls = urls,
                        Database = database
                    };

                    store.Conventions.FindIdentityProperty = memberInfo => memberInfo.Name == "RavenId";
                    store.Initialize();

                    // Register RavenDB Document Store as a Singleton
                    services.AddSingleton<IDocumentStore>(store);

                    // Register IAsyncDocumentSession as Scoped
                    services.AddScoped<IAsyncDocumentSession>(provider =>
                    {
                        var documentStore = provider.GetRequiredService<IDocumentStore>();
                        return documentStore.OpenAsyncSession();
                    });
                });
    }
}
