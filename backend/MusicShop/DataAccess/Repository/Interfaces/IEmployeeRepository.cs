using MusicShop.DataAccess.Entity;

namespace MusicShop.DataAccess.Repository.Interfaces;

public interface IEmployeeRepository
{
    public Task<Employee> GetByIdAsync(int employeeId);
    public Task<List<Employee>> GetAllAsync();
    public Task<Employee> InsertOneAsync(Employee employee);
    public Task<Employee> UpdateOneAsync(Employee employee);
    public Task<Employee> DeleteOneAsync(int employeeId);
}