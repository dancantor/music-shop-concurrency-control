using MusicShop.ConcurrencyControl.Enums;
using MusicShop.DataAccess.Entity;
using MusicShop.DataAccess.Repository.Interfaces;

namespace MusicShop.ConcurrencyControl.Command.Implementations;

public class InsertPurchaseCommand : ICommand
{
    private readonly IInstrumentPurchaseRepository _purchaseRepository;
    private readonly InstrumentPurchase _purchase;

    public InsertPurchaseCommand(IInstrumentPurchaseRepository purchaseRepository, InstrumentPurchase purchase)
    {
        _purchaseRepository = purchaseRepository;
        _purchase = purchase;
    }
    
    public List<Tuple<Table, OperationType>> RequiredTables { get; set; } = new()
    {
        new(Table.Purchase, OperationType.Write)
    };

    public async Task Execute()
    {
        await _purchaseRepository.InsertOneAsync(_purchase);
    }

    public ICommand GetOppositeOperation()
    {
        return new DeletePurchaseCommand(_purchaseRepository, _purchase.Id);
    }
}