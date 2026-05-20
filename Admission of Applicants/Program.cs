using Avalonia;
using System;
using Admission_of_Applicants.DB;
using Admission_of_Applicants.Models;
using Admission_of_Applicants.ViewModels;
using Admission_of_Applicants.Views;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Employees = Admission_of_Applicants.Models.Employees;

namespace Admission_of_Applicants;

sealed class Program
{
    [STAThread]
    public static void Main(string[] args)
    {
        var host = Host.CreateDefaultBuilder().
            ConfigureAppConfiguration((context, config) =>
            {
                config.SetBasePath(AppContext.BaseDirectory)
                    .AddJsonFile("appsettings.json")
                    .AddEnvironmentVariables();
            }).
            ConfigureServices((c,s) =>
            {
                s.Configure<DBConnections>(c.Configuration.
                    GetSection("DatabaseConnection"));
                s.AddTransient<MainWindow>();
                s.AddTransient<MainWindowViewModel>();
                s.AddTransient<EquipmentRepository>();
                s.AddTransient<EmployeesWindow>();
                s.AddTransient<EmployeesViewModel>();
                s.AddTransient<EmployeesRepository>();
                s.AddTransient<TariffsRepository>();
                s.AddTransient<TariffViewModel>();
                s.AddTransient<TariffWindow>();
                s.AddTransient<ClientRepository>();
                s.AddTransient<ClientViewModel>();
                s.AddTransient<ClientWindow>();
                s.AddTransient<TypeDeviceRepository>();
                s.AddTransient<OsTypeRepository>();
            }).
            Build();
        BuildAvaloniaApp(host.Services)
            .StartWithClassicDesktopLifetime(args);
    }

    // Avalonia configuration, don't remove; also used by visual designer.
    public static AppBuilder BuildAvaloniaApp(IServiceProvider serviceProvider)
        => AppBuilder.Configure(()=> new App(serviceProvider))
            .UsePlatformDetect()
            .WithInterFont()
            .LogToTrace();
}