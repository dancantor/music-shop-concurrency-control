using MusicShop.ConcurrencyControl.Enums;
using MusicShop.DataAccess.Entity;
using MusicShop.DataAccess.Repository.Interfaces;

namespace MusicShop.ConcurrencyControl.Command.Implementations;

public class DeleteEmployeeCommand : ICommand
{
    private readonly IEmployeeRepository _employeeRepository;
    private readonly int _employeeId;
    private Employee? _deletedEmployee;

    public DeleteEmployeeCommand(IEmployeeRepository employeeRepository, int employeeId)
    {
        _employeeRepository = employeeRepository;
        _employeeId = employeeId;
    }

    public List<Tuple<Table, OperationType>> RequiredTables { get; set; } = new()
    {
        new(Table.Employee, OperationType.Read),
        new(Table.Employee, OperationType.Write)
    };

    public async Task Execute()
    {
        _deletedEmployee = await _employeeRepository.DeleteOneAsync(_employeeId);
    }

    public ICommand GetOppositeOperation()
    {
        return new InsertEmployeeCommand(_employeeRepository, _deletedEmployee);
    }
}