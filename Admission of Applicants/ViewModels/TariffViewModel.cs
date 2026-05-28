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
    private readonly TariffsRepository _tariffsRepository;
    //public List<Tariffs> _tariffs { get; set; }
    [ObservableProperty] private List<Tariffs> _tariffs;
    [ObservableProperty] private bool _isPaneOpen = true;
    private readonly IServiceProvider _serviceProvider;
    [ObservableProperty] private Tariffs? _selectedTariff;
    
    private Action _closeAction;
    
    public TariffViewModel(IServiceProvider serviceProvider, TariffsRepository tariffsRepository)
    {
        _serviceProvider = serviceProvider;
        //Tariffs = tariffsRepository.GetAllTariffs();
        _tariffsRepository = tariffsRepository;
        Tariffs = tariffsRepository.GetAllTariffs() ?? new List<Tariffs>();
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
    
    [RelayCommand]
    public void MyWindowStart()
    {
        var vm = _serviceProvider.GetRequiredService<MyWindowViewModel>();
        var win = _serviceProvider.GetRequiredService<MyWindow>();
        win.DataContext = vm;
        win.Show();
    }
    
    
    [RelayCommand]
    public void AddTariff()
    {
        var newTariff = new Tariffs
        {
            TariffName = "Новый тариф",
            CostMonth = 0
        };

        var tempList = new List<Tariffs>(Tariffs ?? new List<Tariffs>());
        tempList.Add(newTariff);
        Tariffs = tempList;
    }

    [RelayCommand]
    public void DeleteTariff()
    {
        if (SelectedTariff == null) return;

        if (SelectedTariff.Id != 0)
        {
            _tariffsRepository.DeleteTariff(SelectedTariff.Id);
        }

        var tempList = new List<Tariffs>(Tariffs ?? new List<Tariffs>());
        tempList.Remove(SelectedTariff);
        Tariffs = tempList;
        
        SelectedTariff = null;
    }

    [RelayCommand]
    public void SaveTariffs()
    {
        if (Tariffs == null) return;

        foreach (var tariff in Tariffs)
        {
            if (tariff.Id == 0)
            {
                _tariffsRepository.InsertTariff(tariff);
            }
            else
            {
                _tariffsRepository.UpdateTariff(tariff);
            }
        }
        
        Tariffs = _tariffsRepository.GetAllTariffs() ?? new List<Tariffs>();
    }
}