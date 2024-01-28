using MusicShop.ConcurrencyControl.Enums;
using MusicShop.DataAccess.Entity;
using MusicShop.DataAccess.Repository.Interfaces;

namespace MusicShop.ConcurrencyControl.Command.Implementations;

public class DeletePurchaseCommand : ICommand
{
    private readonly IInstrumentPurchaseRepository _purchaseRepository;
    private readonly int _purchaseId;
    private InstrumentPurchase? _deletedPurchase;

    public DeletePurchaseCommand(IInstrumentPurchaseRepository purchaseRepository, int purchaseId)
    {
        _purchaseRepository = purchaseRepository;
        _purchaseId = purchaseId;
    }
    
    public List<Tuple<Table, OperationType>> RequiredTables { get; set; } = new()
    {
        new(Table.Purchase, OperationType.Read),
        new(Table.Purchase, OperationType.Write)
    };

    public async Task Execute()
    {
        _deletedPurchase = await _purchaseRepository.DeleteOneAsync(_purchaseId);
    }

    public ICommand GetOppositeOperation()
    {
        return new InsertPurchaseCommand(_purchaseRepository, _deletedPurchase);
    }
}