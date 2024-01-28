using MusicShop.ConcurrencyControl.Enums;
using MusicShop.DataAccess.Entity;
using MusicShop.DataAccess.Repository.Interfaces;

namespace MusicShop.ConcurrencyControl.Command.Implementations;

public class InsertEmployeeCommand : ICommand
{
    private readonly IEmployeeRepository _employeeRepository;
    private readonly Employee _employee;

    public InsertEmployeeCommand(IEmployeeRepository employeeRepository, Employee employee)
    {
        _employeeRepository = employeeRepository;
        _employee = employee;
    }

    public List<Tuple<Table, OperationType>> RequiredTables { get; set; } = new()
    {
        new(Table.Employee, OperationType.Write)
    };
    public async Task Execute()
    {
        await _employeeRepository.InsertOneAsync(_employee);
    }

    public ICommand GetOppositeOperation()
    {
        return new DeleteEmployeeCommand(_employeeRepository, _employee.Id);
    }
}