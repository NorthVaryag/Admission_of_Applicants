using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Admission_of_Applicants.DB;
using Admission_of_Applicants.Models;
using Admission_of_Applicants.Models.Types;
using Admission_of_Applicants.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;

namespace Admission_of_Applicants.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    [ObservableProperty] ObservableCollection<Equipment> _equipments;
    [ObservableProperty] List<DeviceType> _deviceTypes;
    [ObservableProperty] List<OsType> _osTypes;
    [ObservableProperty] List<Employees> _employees;
    private readonly IServiceProvider _serviceProvider;
    private readonly EquipmentRepository _equipmentRepository;

    private Action _closeAction;
    
    [ObservableProperty]
    private bool _isPaneOpen = true;

    public MainWindowViewModel(IServiceProvider serviceProvider, EquipmentRepository equipmentRepository,
        TypeDeviceRepository typeDeviceRepository, OsTypeRepository osTypeRepository,
        EmployeesRepository employeesRepository)
    {
        _serviceProvider = serviceProvider;
        _equipmentRepository = equipmentRepository;
        Equipments = new ObservableCollection<Equipment>(_equipmentRepository.GetAllEquipment());
        _deviceTypes = typeDeviceRepository.GetTypeDevices();
        _osTypes = osTypeRepository.GetTypeOs();
        _employees = employeesRepository.GetAllEmployees();
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
    public void AddEquipment()
    {
        var emptyEquipment = new Equipment 
        { 
            /*DeviceType = _deviceTypes.Count > 0 ? _deviceTypes[0].DeviceTypeName : "Сервер", 
            Os = _osTypes.Count > 0 ? _osTypes[0].OsName : "Без ОС", 
            // Подставляем имя первого сотрудника из базы, чтобы репозиторий не получил null/пустоту:
            EmployeeName = _employees.Count > 0 ? _employees[0].FirstName : "Администратор", 
            EmployeeLastName = "",
            Ram = 0,
            Vram = 0,
            Storage = 0,
            NetworkThroughput = 0*/
        };
        Equipments.Add(emptyEquipment);
    }

    [RelayCommand]
    public void SaveEquipment()
    {
        foreach (var equipment in Equipments)
        {
            if (equipment.Id == 0)
            {
                _equipmentRepository.InsertEquipment(equipment);
            }
            else
            {
                _equipmentRepository.UpdateEquipment(equipment);
            }
        }
        Equipments = new ObservableCollection<Equipment>(_equipmentRepository.GetAllEquipment());
    }
}