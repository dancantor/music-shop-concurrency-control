using Microsoft.AspNetCore.Mvc;
using MusicShop.Business.Model;
using MusicShop.Business.Service.Interfaces;

namespace MusicShop.Controllers;

[ApiController]
[Route("api/employees")]
public class EmployeeController : ControllerBase
{
    private readonly IEmployeeService _employeeService;

    public EmployeeController(IEmployeeService employeeService)
    {
        _employeeService = employeeService;
    }

    [HttpGet]
    public async Task<ActionResult<List<EmployeeModel>>> GetAllEmployees()
    {
        return Ok(await _employeeService.GetAllAsync());
    }

    [HttpPost]
    public async Task<ActionResult<EmployeeModel>> InsertOneEmployee([FromBody] EmployeeModel employeeModel)
    {
        return CreatedAtAction(nameof(GetAllEmployees), await _employeeService.InsertOneAsync(employeeModel));
    }
}