using Microsoft.EntityFrameworkCore;
using MusicShop.DataAccess.Context;
using MusicShop.DataAccess.Entity;
using MusicShop.DataAccess.Repository.Interfaces;

namespace MusicShop.DataAccess.Repository;

public class EmployeeRepository : IEmployeeRepository
{
    private readonly EmployeeContext _employeeContext;
    public EmployeeRepository(EmployeeContext employeeContext)
    {
        _employeeContext = employeeContext;
    }

    public async Task<Employee> GetByIdAsync(int employeeId)
    {
        var searchedEmployee =
            await _employeeContext.Employees.FirstOrDefaultAsync(employee => employee.Id == employeeId);
        if (searchedEmployee is null)
        {
            throw new KeyNotFoundException("Couldn't find the employee with that id key");
        }

        return searchedEmployee;
    }

    public async Task<List<Employee>> GetAllAsync()
    {
        return await _employeeContext.Employees.ToListAsync();
    }

    public async Task<Employee> InsertOneAsync(Employee employee)
    {
        var insertedEmployee = await _employeeContext.Employees.AddAsync(employee);
        await _employeeContext.SaveChangesAsync();
        return insertedEmployee.Entity;
    }

    public async Task<Employee> UpdateOneAsync(Employee employee)
    {
        var initialEmployee = await GetByIdAsync(employee.Id);

        initialEmployee.Position = employee.Position ?? initialEmployee.Position;
        initialEmployee.Name = employee.Name ?? initialEmployee.Name;
        initialEmployee.DateOfBirth = employee.DateOfBirth ?? initialEmployee.DateOfBirth;
        await _employeeContext.SaveChangesAsync();
        return initialEmployee;
    }

    public async Task<Employee> DeleteOneAsync(int employeeId)
    {
        var deletedEmployee = await GetByIdAsync(employeeId);
        if (deletedEmployee is null)
        {
            throw new KeyNotFoundException("Couldn't find the employee with that id key");
        }

        _employeeContext.Employees.Remove(deletedEmployee);
        await _employeeContext.SaveChangesAsync();
        return deletedEmployee;
    }
}