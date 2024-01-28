using MusicShop.DataAccess.Entity;

namespace MusicShop.Business.Model;

public class EmployeeModel
{
    public EmployeeModel()
    {
    }

    public EmployeeModel(Employee employeeEntity)
    {
        Id = employeeEntity.Id;
        Name = employeeEntity.Name;
        DateOfBirth = employeeEntity.DateOfBirth;
        Position = employeeEntity.Position;
    }

    public int? Id { get; set; }

    public string? Name { get; set; }

    public DateOnly? DateOfBirth { get; set; }

    public int? Position { get; set; }
}