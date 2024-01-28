using MusicShop.ConcurrencyControl.Enums;
using MusicShop.DataAccess.Entity;
using MusicShop.DataAccess.Repository.Interfaces;

namespace MusicShop.ConcurrencyControl.Command.Implementations;

public class UpdateEmployeePositionCommand : ICommand
{
    private readonly IEmployeeRepository _employeeRepository;
    private readonly int _employeeId;
    private readonly int _position;
    private int? _previousPosition;

    public UpdateEmployeePositionCommand(IEmployeeRepository employeeRepository, int employeeId, int position)
    {
        _employeeRepository = employeeRepository;
        _employeeId = employeeId;
        _position = position;
    }
    
    public List<Tuple<Table, OperationType>> RequiredTables { get; set; } = new()
    {
        new(Table.Employee, OperationType.Read),
        new(Table.Employee, OperationType.Write)
    };

    public async Task Execute()
    {
        _previousPosition = (await _employeeRepository.GetByIdAsync(_employeeId)).Position;
        await _employeeRepository.UpdateOneAsync(new Employee
        {
            Id = _employeeId,
            Position = _position,
        });
    }

    public ICommand GetOppositeOperation()
    {
        return new UpdateEmployeePositionCommand(_employeeRepository, _employeeId, _previousPosition.Value);
    }
}