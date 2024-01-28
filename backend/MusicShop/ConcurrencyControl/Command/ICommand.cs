using MusicShop.ConcurrencyControl.Enums;

namespace MusicShop.ConcurrencyControl.Command;

public interface ICommand
{
    public List<Tuple<Table, OperationType>> RequiredTables { get; set; }
    public Task Execute();
    public ICommand GetOppositeOperation();
}