using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NewsAggregationFE.Controllers;
using NewsAggregationFE.Controllers.Interfaces;
using NewsAggregationFE.Core.Interfaces;
using NewsAggregationFE.Core.Models;
using NewsAggregationFE.Presentation;
using NewsAggregationFE.Services;
using NewsAggregationFE.Services.Interfaces;
using NewsAggregationFE.State;

namespace NewsAggregationFE
{
    internal class Program
    {
        public static async Task Main(string[] args)
        {
            var services = ConfigureServices();
            var serviceProvider = services.BuildServiceProvider();
            AppController appController = serviceProvider.GetRequiredService<AppController>();
            await appController.Run();
        }

        private static IServiceCollection ConfigureServices()
        {
            var services = new ServiceCollection();

            // Configuration
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appSettings.json", optional: false, reloadOnChange: true)
                .Build();

            var appConfiguration = new AppConfiguration();
            configuration.Bind(appConfiguration);
            services.AddSingleton(appConfiguration);

            // Register services
            services.AddSingleton<IConsoleView, ConsoleView>();
            services.AddSingleton<IWelcomeScreen, WelcomeScreen>();
            services.AddSingleton<ILoginScreen, LoginScreen>();
            services.AddSingleton<IAuthService, AuthService>();
            services.AddSingleton<IAuthController, AuthController>();
            services.AddSingleton<IAdminController, AdminController>();
            services.AddSingleton<IUserController, UserController>();
            services.AddSingleton<IUserService, UserService>();
            services.AddSingleton<IAdminService, AdminService>();
            services.AddSingleton<AppController>();
            services.AddSingleton<AppState>();

            return services;
        }
    }
}