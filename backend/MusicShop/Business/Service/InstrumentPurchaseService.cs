using MusicShop.Business.Model;
using MusicShop.Business.Service.Interfaces;
using MusicShop.ConcurrencyControl.Enums;
using MusicShop.ConcurrencyControl.Models;
using MusicShop.ConcurrencyControl.Services;
using MusicShop.Controllers.DTO;
using MusicShop.DataAccess.Entity;
using MusicShop.DataAccess.Repository.Interfaces;

namespace MusicShop.Business.Service;

public class InstrumentPurchaseService : IInstrumentPurchaseService
{
    private readonly IInstrumentPurchaseRepository _purchaseRepository;
    private readonly IEmployeeRepository _employeeRepository;
    private readonly IMusicalInstrumentRepository _instrumentRepository;
    private readonly ConcurrencyControlService _concurrencyControlService;
    private readonly int _nrOfRequiredValuesBeforePromotion = 3;

    public InstrumentPurchaseService(IInstrumentPurchaseRepository purchaseRepository, IEmployeeRepository employeeRepository, IMusicalInstrumentRepository instrumentRepository, ConcurrencyControlService concurrencyControlService)
    {
        _purchaseRepository = purchaseRepository;
        _employeeRepository = employeeRepository;
        _instrumentRepository = instrumentRepository;
        _concurrencyControlService = concurrencyControlService;
    }

    public async Task<List<InstrumentPurchaseDTO>> GetAllAsync()
    {
        var transaction = new Transaction(new List<Tuple<Table, OperationType>>()
        {
            new(Table.Purchase, OperationType.Read),
            new(Table.Employee, OperationType.Read),
            new(Table.Instrument, OperationType.Read),
        });
        _concurrencyControlService.AddNewTransaction(transaction);
        _concurrencyControlService.BlockTablesForTransaction(transaction);
        var purchases = await _purchaseRepository.GetAllAsync();
        var employeeNamesById =
            (await _employeeRepository.GetAllAsync()).ToDictionary(employee => employee.Id, employee => employee.Name);
        var purchasesDto = purchases.Select(purchase => new InstrumentPurchaseDTO
        {
            Id = purchase.Id,
            InstrumentId = purchase.InstrumentId,
            InstrumentName = purchase.Instrument?.Name,
            DateSold = purchase.DateSold,
            EmployeeId = purchase.EmployeeId,
            EmployeeName = employeeNamesById[purchase.EmployeeId ?? 0]
        }).ToList();
        
        _concurrencyControlService.CommitTransaction(transaction);
        
        return purchasesDto;
    }

    public async Task<InstrumentPurchaseDTO> BuyInstrumentAsync(InstrumentPurchaseModel purchaseModel)
    {
        if (purchaseModel.InstrumentId is null || purchaseModel.EmployeeId is null)
        {
            throw new KeyNotFoundException(
                "The id of the employee and of the instrument are required for making a purchase");
        }

        var transaction = new Transaction(new List<Tuple<Table, OperationType>>()
        {
            new(Table.Purchase, OperationType.Write),
            new(Table.Instrument, OperationType.Read),
            new(Table.Instrument, OperationType.Write),
            new(Table.Purchase, OperationType.Read),
            new(Table.Employee, OperationType.Write),
            new(Table.Employee, OperationType.Read)
        });
        _concurrencyControlService.AddNewTransaction(transaction);
        _concurrencyControlService.BlockTablesForTransaction(transaction);
        
        var insertedPurchase = await _purchaseRepository.InsertOneAsync(new InstrumentPurchase
        {
            Id = 0,
            InstrumentId = purchaseModel.InstrumentId,
            DateSold = DateOnly.FromDateTime(DateTime.Now),
            EmployeeId = purchaseModel.EmployeeId,
        });
        
        var currentInstrumentStock = (await _instrumentRepository.GetByIdAsync(purchaseModel.InstrumentId.Value)).ItemsStock ?? 1;
        await _instrumentRepository.UpdateOneAsync(new MusicalInstrument()
        {
            Id = purchaseModel.InstrumentId.Value,
            ItemsStock = currentInstrumentStock - 1
        });
        
        var instrumentsSoldByEmployeeCount =
            (await _purchaseRepository.GetAllAsync()).Count(purchase => purchase.EmployeeId == purchaseModel.EmployeeId);
        await _employeeRepository.UpdateOneAsync(new Employee()
        {
            Id = purchaseModel.EmployeeId.Value,
            Position = instrumentsSoldByEmployeeCount / _nrOfRequiredValuesBeforePromotion + 1
        });
        var employeeName = (await _employeeRepository.GetByIdAsync(insertedPurchase.EmployeeId ?? 0)).Name;
        
        _concurrencyControlService.CommitTransaction(transaction);
        
        return new InstrumentPurchaseDTO
        {
            Id = insertedPurchase.Id,
            InstrumentId = insertedPurchase.InstrumentId,
            InstrumentName = insertedPurchase.Instrument?.Name,
            DateSold = insertedPurchase.DateSold,
            EmployeeId = insertedPurchase.EmployeeId,
            EmployeeName = employeeName
        };
    }

    public async Task<InstrumentPurchaseDTO> ReturnInstrumentAsync(int purchaseId)
    {
        var transaction = new Transaction(new List<Tuple<Table, OperationType>>()
        {
            new(Table.Purchase, OperationType.Write),
            new(Table.Instrument, OperationType.Read),
            new(Table.Instrument, OperationType.Write),
            new(Table.Purchase, OperationType.Read),
            new(Table.Employee, OperationType.Write),
            new(Table.Employee, OperationType.Read)
        });
        _concurrencyControlService.AddNewTransaction(transaction);
        _concurrencyControlService.BlockTablesForTransaction(transaction);
        
        var deletedPurchase = await _purchaseRepository.DeleteOneAsync(purchaseId);
        
        var currentInstrumentStock = (await _instrumentRepository.GetByIdAsync(deletedPurchase.InstrumentId.Value)).ItemsStock ?? 1;
        await _instrumentRepository.UpdateOneAsync(new MusicalInstrument()
        {
            Id = deletedPurchase.InstrumentId.Value,
            ItemsStock = currentInstrumentStock + 1
        });
        
        var instrumentsSoldByEmployeeCount =
            (await _purchaseRepository.GetAllAsync()).Count(purchase => purchase.EmployeeId == deletedPurchase.EmployeeId);
        await _employeeRepository.UpdateOneAsync(new Employee()
        {
            Id = deletedPurchase.EmployeeId.Value,
            Position = instrumentsSoldByEmployeeCount / _nrOfRequiredValuesBeforePromotion + 1
        });
        var employeeName = (await _employeeRepository.GetByIdAsync(deletedPurchase.EmployeeId ?? 0)).Name;
        
        _concurrencyControlService.CommitTransaction(transaction);
        
        return new InstrumentPurchaseDTO
        {
            Id = deletedPurchase.Id,
            InstrumentId = deletedPurchase.InstrumentId,
            InstrumentName = deletedPurchase.Instrument?.Name,
            DateSold = deletedPurchase.DateSold,
            EmployeeId = deletedPurchase.EmployeeId,
            EmployeeName = employeeName
        };
    }
}