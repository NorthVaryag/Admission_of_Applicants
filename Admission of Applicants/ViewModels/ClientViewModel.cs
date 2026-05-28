using System;
using System.Collections.Generic;
using Admission_of_Applicants.DB;
using Admission_of_Applicants.Models;
using Admission_of_Applicants.Models.Types;
using Admission_of_Applicants.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;

namespace Admission_of_Applicants.ViewModels;

public partial class ClientViewModel : ViewModelBase
{
    [ObservableProperty] private List<Tariffs> _tariffs;
    [ObservableProperty] private List<ClientType> _clientTypes;
    
    //public List<Client> Clients { get; set; }
    [ObservableProperty] private List<Client> _clients;
    [ObservableProperty] private bool _isPaneOpen = true;
    [ObservableProperty] private Client? _selectedClient;
    private readonly IServiceProvider _serviceProvider;
    
    private Action _closeAction;
    private readonly ClientRepository _clientRepository;

    public ClientViewModel(IServiceProvider serviceProvider, MainWindowViewModel mainWindowViewModel,
        ClientRepository clientRepository, TariffsRepository tariffsRepository, ClientTypeRepository clientTypeRepository)
    {
        _isPaneOpen = mainWindowViewModel.IsPaneOpen;
        _serviceProvider = serviceProvider;
        _clientRepository = clientRepository;
        //Clients =  clientRepository.GetAllClient();
        
        Clients = clientRepository.GetAllClient() ?? new List<Client>();
        Tariffs = tariffsRepository.GetAllTariffs() ?? new List<Tariffs>();
        ClientTypes = clientTypeRepository.GetClientTypes() ?? new List<ClientType>();
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
    
    [RelayCommand]
    public void MyWindowStart()
    {
        var vm = _serviceProvider.GetRequiredService<MyWindowViewModel>();
        var win = _serviceProvider.GetRequiredService<MyWindow>();
        win.DataContext = vm;
        win.Show();
    }
    
    
    [RelayCommand]
    public void AddClient()
    {
        var newClient = new Client
        {
            NameType = "-",
            DisplayName = "Новый клиент",
            TariffName = "-",
            IsCustom = 0,
            MonthlyPayment = 0
        };

        var tempList = new List<Client>(Clients ?? new List<Client>());
        tempList.Add(newClient);
        Clients = tempList; 
    }

    [RelayCommand]
    public void DeleteClient()
    {
        if (SelectedClient == null) return;

        if (SelectedClient.Id != 0)
        {
            _clientRepository.DeleteClient(SelectedClient.Id);
        }

        var tempList = new List<Client>(Clients ?? new List<Client>());
        tempList.Remove(SelectedClient);
        Clients = tempList;
        
        SelectedClient = null;
    }

    [RelayCommand]
    public void SaveClient()
    {
        if (Clients == null) return;

        foreach (var client in Clients)
        {
            if (client.Id == 0)
            {
                _clientRepository.InsertClient(client);
            }
            else
            {
                _clientRepository.UpdateClient(client);
            }
        }
        
        Clients = _clientRepository.GetAllClient() ?? new List<Client>();
    }
}