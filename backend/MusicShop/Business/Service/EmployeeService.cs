using MusicShop.Business.Model;
using MusicShop.Business.Service.Interfaces;
using MusicShop.ConcurrencyControl.Enums;
using MusicShop.ConcurrencyControl.Models;
using MusicShop.ConcurrencyControl.Services;
using MusicShop.DataAccess.Entity;
using MusicShop.DataAccess.Repository.Interfaces;

namespace MusicShop.Business.Service;

public class EmployeeService : IEmployeeService
{
    private readonly IEmployeeRepository _employeeRepository;
    private readonly ConcurrencyControlService _concurrencyControlService;
    private readonly AbortTransactionService _abortTransactionService;


    public EmployeeService(IEmployeeRepository employeeRepository, ConcurrencyControlService concurrencyControlService, AbortTransactionService abortTransactionService)
    {
        _employeeRepository = employeeRepository;
        _concurrencyControlService = concurrencyControlService;
        _abortTransactionService = abortTransactionService;
    }

    public async Task<List<EmployeeModel>> GetAllAsync()
    {
        var transaction = new Transaction(new List<Tuple<Table, OperationType>>()
        {
            new(Table.Employee, OperationType.Read),
        });
        _concurrencyControlService.AddNewTransaction(transaction);
        _concurrencyControlService.BlockTablesForTransaction(transaction);
        
        var employeeList = (await _employeeRepository.GetAllAsync()).Select(employeeEntity => new EmployeeModel(employeeEntity))
            .ToList();

        _concurrencyControlService.CommitTransaction(transaction);
        return employeeList;
    }

    public async Task<EmployeeModel> InsertOneAsync(EmployeeModel employeeModel)
    {
        var transaction = new Transaction(new List<Tuple<Table, OperationType>>()
        {
            new(Table.Employee, OperationType.Write),
        });
        _concurrencyControlService.AddNewTransaction(transaction);
        _concurrencyControlService.BlockTablesForTransaction(transaction);
        
        var employeeEntity = new Employee
        {
            Id = 0,
            Name = employeeModel.Name,
            DateOfBirth = employeeModel.DateOfBirth,
            Position = employeeModel.Position
        };
        var insertedEmployee = new EmployeeModel(await _employeeRepository.InsertOneAsync(employeeEntity));

        _concurrencyControlService.CommitTransaction(transaction);
        return insertedEmployee;
    }
}