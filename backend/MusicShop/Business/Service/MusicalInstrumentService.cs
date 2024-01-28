using MusicShop.Business.Model;
using MusicShop.Business.Service.Interfaces;
using MusicShop.ConcurrencyControl.Enums;
using MusicShop.ConcurrencyControl.Models;
using MusicShop.ConcurrencyControl.Services;
using MusicShop.DataAccess.Entity;
using MusicShop.DataAccess.Repository.Interfaces;

namespace MusicShop.Business.Service;

public class MusicalInstrumentService : IMusicalInstrumentService
{
    private readonly IMusicalInstrumentRepository _musicalInstrumentRepository;
    private readonly ConcurrencyControlService _concurrencyControlService;

    public MusicalInstrumentService(IMusicalInstrumentRepository musicalInstrumentRepository, ConcurrencyControlService concurrencyControlService)
    {
        _musicalInstrumentRepository = musicalInstrumentRepository;
        _concurrencyControlService = concurrencyControlService;
    }

    public async Task<List<MusicalInstrumentModel>> GetAllAsync()
    {
        var transaction = new Transaction(new List<Tuple<Table, OperationType>>()
        {
            new(Table.Instrument, OperationType.Read),
        });
        _concurrencyControlService.AddNewTransaction(transaction);
        _concurrencyControlService.BlockTablesForTransaction(transaction);
        
        var instrumentsList = (await _musicalInstrumentRepository.GetAllAsync()).Select(instrument => new MusicalInstrumentModel(instrument))
            .ToList();

        _concurrencyControlService.CommitTransaction(transaction);
        return instrumentsList;
    }

    public async Task<MusicalInstrumentModel> InsertOneAsync(MusicalInstrumentModel musicalInstrumentModel)
    {
        var transaction = new Transaction(new List<Tuple<Table, OperationType>>()
        {
            new(Table.Instrument, OperationType.Write),
        });
        _concurrencyControlService.AddNewTransaction(transaction);
        _concurrencyControlService.BlockTablesForTransaction(transaction);

        var instrumentEntity = new MusicalInstrument()
        {
            Id = 0,
            Name = musicalInstrumentModel.Name,
            ItemsStock = musicalInstrumentModel.ItemsStock,
            Price = musicalInstrumentModel.Price
        };
        var insertedInstrument = new MusicalInstrumentModel(await _musicalInstrumentRepository.InsertOneAsync(instrumentEntity));
       
        _concurrencyControlService.CommitTransaction(transaction);
        return insertedInstrument;
    }

    public async Task<MusicalInstrumentModel> UpdateItemStock(int instrumentId, int newStock)
    {
        var transaction = new Transaction(new List<Tuple<Table, OperationType>>()
        {
            new(Table.Instrument, OperationType.Read),
            new(Table.Instrument, OperationType.Write),
        });
        _concurrencyControlService.AddNewTransaction(transaction);
        _concurrencyControlService.BlockTablesForTransaction(transaction);
        
        var updatedInstrument = new MusicalInstrument()
        {
            Id = instrumentId,
            ItemsStock = newStock
        };
        var updatedInstrumentResult =
            new MusicalInstrumentModel(await _musicalInstrumentRepository.UpdateOneAsync(updatedInstrument));
        
        _concurrencyControlService.CommitTransaction(transaction);

        return updatedInstrumentResult;
    }
}