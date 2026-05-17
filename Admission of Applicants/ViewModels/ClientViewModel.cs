using System;
using System.Collections.Generic;
using Admission_of_Applicants.DB;
using Admission_of_Applicants.Models;
using Admission_of_Applicants.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;

namespace Admission_of_Applicants.ViewModels;

public partial class ClientViewModel : ViewModelBase
{
    public List<Client> Clients { get; set; }
    [ObservableProperty]
    private bool _isPaneOpen = true;
    private readonly IServiceProvider _serviceProvider;
    
    private Action _closeAction;
    
    public ClientViewModel(IServiceProvider serviceProvider, MainWindowViewModel mainWindowViewModel, ClientRepository clientRepository)
    {
        _isPaneOpen = mainWindowViewModel.IsPaneOpen;
        _serviceProvider = serviceProvider;
        Clients =  clientRepository.GetAllClient();
    }


    [RelayCommand]
    private void TogglePane() => IsPaneOpen = !IsPaneOpen;
    
    public void CloseAction(Action action)
    {
        _closeAction  = action;
    }

    [RelayCommand]
    public void EmployeesWindowStart()
    {
        var vm = ActivatorUtilities.CreateInstance<EmployeesViewModel>(_serviceProvider);
        var  win = _serviceProvider.GetRequiredService<EmployeesWindow>();
        win.DataContext = vm;
        win.Show();
        vm.CloseAction(win.Close);
        _closeAction?.Invoke();
    }
    
    [RelayCommand]
    public void TariffWindowStart()
    {
        var vm = ActivatorUtilities.CreateInstance<TariffViewModel>(_serviceProvider);
        var  win = _serviceProvider.GetRequiredService<TariffWindow>();
        win.DataContext = vm;
        win.Show();
        vm.CloseAction(win.Close);
        _closeAction?.Invoke();
    }
    
    [RelayCommand]
    public void MainWindowStart()
    {
        var vm = ActivatorUtilities.CreateInstance<MainWindowViewModel>(_serviceProvider);
        var win = _serviceProvider.GetRequiredService<MainWindow>();
        win.DataContext = vm;
        win.Show();
        vm.CloseAction(win.Close);
        _closeAction?.Invoke();
    }
}