using System;
using System.Collections.Generic;
using Admission_of_Applicants.DB;
using Admission_of_Applicants.Models;
using Admission_of_Applicants.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;

namespace Admission_of_Applicants.ViewModels;

public partial class EmployeesViewModel : ViewModelBase
{
    public List<Employees> Employees { get; set; }
    [ObservableProperty]
    private bool _isPaneOpen = true;
    private readonly IServiceProvider _serviceProvider;
    
    private Action _closeAction;
    
    public EmployeesViewModel(IServiceProvider serviceProvider, MainWindowViewModel mainWindowViewModel, EmployeesRepository  employeesRepository)
    {
        _isPaneOpen = mainWindowViewModel.IsPaneOpen;
        _serviceProvider = serviceProvider;
        Employees =  employeesRepository.GetAllEmployees();
    }


    [RelayCommand]
    private void TogglePane() => IsPaneOpen = !IsPaneOpen;
    
    public void CloseAction(Action action)
    {
        _closeAction  = action;
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