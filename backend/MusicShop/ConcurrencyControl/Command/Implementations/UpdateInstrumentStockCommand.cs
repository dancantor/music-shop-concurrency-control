using MusicShop.ConcurrencyControl.Enums;
using MusicShop.DataAccess.Entity;
using MusicShop.DataAccess.Repository.Interfaces;

namespace MusicShop.ConcurrencyControl.Command.Implementations;

public class UpdateInstrumentStockCommand : ICommand
{
    private readonly IMusicalInstrumentRepository _instrumentRepository;
    private readonly int _instrumentId;
    private readonly int _newStock;
    private int? _previousStock;

    public UpdateInstrumentStockCommand(IMusicalInstrumentRepository instrumentRepository, int newStock, int instrumentId)
    {
        _instrumentRepository = instrumentRepository;
        _newStock = newStock;
        _instrumentId = instrumentId;
    }
    
    public List<Tuple<Table, OperationType>> RequiredTables { get; set; } = new()
    {
        new(Table.Instrument, OperationType.Read),
        new(Table.Instrument, OperationType.Write)
    };

    public async Task Execute()
    {
        _previousStock = (await _instrumentRepository.GetByIdAsync(_instrumentId)).ItemsStock;
        await _instrumentRepository.UpdateOneAsync(new MusicalInstrument
        {
            Id = _instrumentId,
            ItemsStock = _newStock,
        });
    }

    public ICommand GetOppositeOperation()
    {
        return new UpdateInstrumentStockCommand(_instrumentRepository, _instrumentId, _previousStock.Value);
    }
}