using MusicShop.ConcurrencyControl.Enums;
using MusicShop.DataAccess.Entity;
using MusicShop.DataAccess.Repository.Interfaces;

namespace MusicShop.ConcurrencyControl.Command.Implementations;

public class DeleteInstrumentCommand : ICommand
{
    private readonly IMusicalInstrumentRepository _instrumentRepository;
    private readonly int _instrumentId;
    private MusicalInstrument? _deletedInstrument;

    public DeleteInstrumentCommand(IMusicalInstrumentRepository instrumentRepository, int instrumentId)
    {
        _instrumentRepository = instrumentRepository;
        _instrumentId = instrumentId;
    }
    
    public List<Tuple<Table, OperationType>> RequiredTables { get; set; } = new()
    {
        new(Table.Instrument, OperationType.Read),
        new(Table.Instrument, OperationType.Write)
    };

    public async Task Execute()
    {
        _deletedInstrument = await _instrumentRepository.DeleteOneAsync(_instrumentId);
    }

    public ICommand GetOppositeOperation()
    {
        return new InsertInstrumentCommand(_instrumentRepository, _deletedInstrument);
    }
}