using System;
using System.Collections.Generic;
using Admission_of_Applicants.DB;
using Admission_of_Applicants.Models;
using Admission_of_Applicants.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;

namespace Admission_of_Applicants.ViewModels;

public partial class TariffViewModel : ViewModelBase
{
    public List<Tariffs> Tariffs { get; set; }
    [ObservableProperty]
    private bool _isPaneOpen = true;
    private readonly IServiceProvider _serviceProvider;
    
    private Action _closeAction;
    
    public TariffViewModel(IServiceProvider serviceProvider, TariffsRepository tariffsRepository)
    {
        _serviceProvider = serviceProvider;
        Tariffs = tariffsRepository.GetAllTariffs();
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
    public void ClientWindowStart()
    {
        var vm = ActivatorUtilities.CreateInstance<ClientViewModel>(_serviceProvider);
        var  win = _serviceProvider.GetRequiredService<ClientWindow>();
        win.DataContext = vm;
        win.Show();
        vm.CloseAction(win.Close);
        _closeAction?.Invoke();
    }
}