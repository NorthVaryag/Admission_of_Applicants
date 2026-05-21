using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Admission_of_Applicants.DB;
using Admission_of_Applicants.Models;
using Admission_of_Applicants.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;

namespace Admission_of_Applicants.ViewModels;

public partial class EmployeesViewModel : ViewModelBase
{
    public List<string> Genders { get; set; } = new() { "Мужской", "Женский" };
    
    private readonly EmployeesRepository _employeesRepository;
    [ObservableProperty] private ObservableCollection<Employees> _employeesList;
    [ObservableProperty] private Employees? _selectedEmployee;
    [ObservableProperty] private List<Employees> _employees;
    
    //public List<Employees> Employees { get; set; }
    [ObservableProperty]
    private bool _isPaneOpen = true;
    private readonly IServiceProvider _serviceProvider;
    
    private Action _closeAction;
    
    public EmployeesViewModel(IServiceProvider serviceProvider, MainWindowViewModel mainWindowViewModel,
        EmployeesRepository  employeesRepository)
    {
        _employeesRepository = employeesRepository;
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
    public void DeleteEmployee()
    {
        if (SelectedEmployee == null) return;

        if (SelectedEmployee.Id != 0)
        {
            _employeesRepository.DeleteEmployee(SelectedEmployee.Id);
        }

        var tempList = new List<Employees>(Employees ?? new List<Employees>());
        tempList.Remove(SelectedEmployee);
        Employees = tempList;
        
        SelectedEmployee = null;
    }

    [RelayCommand]
    public void AddEmployee()
    {
        var newEmployees = new Employees
        {
            FirstName = "-",
            LastName = "-",
            Surname = "-",
            Gender = "Мужской",
            Age = 0,
            NumberPhone = "-",
            Email = "-",
            Address = "-",
            Experience = 0,
            Salary = 0
        };
        var tempList = new List<Employees>(Employees);
        tempList.Add(newEmployees);
        Employees = tempList;
    }

    [RelayCommand]
    public void SaveEmployees()
    {
        if (Employees == null) return;

        foreach (var emp in Employees)
        {
            if (emp.Id == 0)
            {
                _employeesRepository.InsertEmployee(emp);
            }
            else
            {
                _employeesRepository.UpdateEmployee(emp);
            }
        }
        
        Employees = _employeesRepository.GetAllEmployees() ?? new List<Employees>();
    }
}