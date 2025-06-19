using Microsoft.Extensions.DependencyInjection;
using NewsAggregationFE.Controllers;
using NewsAggregationFE.Controllers.Interfaces;
using NewsAggregationFE.Core.Interfaces;
using NewsAggregationFE.Presentation;
using NewsAggregationFE.Services;
using NewsAggregationFE.Services.Interfaces;
using NewsAggregationFE.State;

public class Program
{
    public static void Main(string[] args)
    {
        //var config = new ConfigurationBuilder()
        //      .SetBasePath(AppContext.BaseDirectory)
        //      .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
        //      .Build();

        var serviceProvider = new ServiceCollection()
            .AddTransient<AppController>()
            .AddTransient<IConsoleView, ConsoleView>()
            .AddTransient<IAuthController, AuthController>()
            .AddTransient<IAuthService, AuthService>()
            .AddSingleton<AppState>()
            .BuildServiceProvider();

        var app = serviceProvider.GetRequiredService<AppController>();
        app.Run();
    }
}