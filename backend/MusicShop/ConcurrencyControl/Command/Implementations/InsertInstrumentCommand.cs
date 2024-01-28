using MusicShop.ConcurrencyControl.Enums;
using MusicShop.DataAccess.Entity;
using MusicShop.DataAccess.Repository.Interfaces;

namespace MusicShop.ConcurrencyControl.Command.Implementations;

public class InsertInstrumentCommand : ICommand
{
    private readonly IMusicalInstrumentRepository _instrumentRepository;
    private readonly MusicalInstrument _instrument;

    public InsertInstrumentCommand(IMusicalInstrumentRepository instrumentRepository, MusicalInstrument instrument)
    {
        _instrumentRepository = instrumentRepository;
        _instrument = instrument;
    }

    public List<Tuple<Table, OperationType>> RequiredTables { get; set; } = new()
    {
        new(Table.Instrument, OperationType.Write)
    };
    public async Task Execute()
    {
        await _instrumentRepository.InsertOneAsync(_instrument);
    }

    public ICommand GetOppositeOperation()
    {
        return new DeleteInstrumentCommand(_instrumentRepository, _instrument.Id);
    }
}