using MusicShop.Business.Model;
using MusicShop.DataAccess.Repository.Interfaces;

namespace MusicShop.Business.Service.Interfaces;

public interface IEmployeeService
{
    public Task<List<EmployeeModel>> GetAllAsync();
    public Task<EmployeeModel> InsertOneAsync(EmployeeModel employeeModel);
}