using EmployeeCRUD.Application.Command.Employees;
using EmployeeCRUD.Application.Dtos.Employees;
using EmployeeCRUD.Application.Queries.Employees;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeCRUD.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController(ISender sender) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> AddEmployeeAsync([FromBody] EmployeeDto employee)
        {
            var result = await sender.Send(new AddEmployeeCommand(employee));
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllEmployeesAsync()
        {
            var result = await sender.Send(new GetAllEmployeesQuery());
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetEmployeeByIdAsync(Guid id)
        {
            var result = await sender.Send(new GetEmployeeByIdQuery(id));
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEmployeeAsync(Guid id, [FromBody] EmployeeDto employee)
        {
            var result = await sender.Send(new UpdateEmployeeCommand(id, employee));
            return Ok(result);
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> PatchEmployeeAsync(Guid id, [FromBody] EmployeePatchDto employee)
        {
            var result = await sender.Send(new PatchEmployeeCommand(id, employee));
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployeeAsync(Guid id)
        {
            var result = await sender.Send(new DeleteEmployeeCommand(id));
            return Ok(result);
        }
        [HttpGet("using-sp")]
        public async Task<IActionResult> GetAllEmployeesUsingStoredProcedureAsync()
        {
            var result = await sender.Send(new GetAllEmployeesSPQuery());
            return Ok(result);
        }

        [HttpGet("GetById-using-SP/{id}")]
        public async Task<IActionResult> GetEmployeeByIdUsingStoredProcedureAsync(Guid id)
        {
            var result = await sender.Send(new GetEmployeeByIdSpQuery(id));
            return Ok(result);
        }

        [HttpDelete("delete-using-SP/{id}")]
        public async Task<IActionResult> DeleteEmployeeUsingStoredProcedureAsync(Guid id)
        {
            var result = await sender.Send(new DeleteEmployeeSpCommand(id));
            return Ok(result);
        }
    }
}
