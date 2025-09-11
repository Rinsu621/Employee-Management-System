
using EmployeeCRUD.Application.EmployeeModule.Commands;
using EmployeeCRUD.Application.EmployeeModule.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeCRUD.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController(ISender sender) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> AddEmployeeAsync( AddEmployeeCommand command)
        {
            var result = await sender.Send(command);
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

        [HttpPut("/update")]
        public async Task<IActionResult> UpdateEmployeeAsync(UpdateEmployeeCommand command)
        {
            var result = await sender.Send(command);
            return Ok(result);
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> PatchEmployeeAsync(PatchEmployeeCommand command)
        {
            var result = await sender.Send(command);
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
        public async Task<IActionResult> GetEmployeeByIdUsingStoredProcedureAsync( Guid id)
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

        [HttpPost("using-sp")]
        public async Task<IActionResult> AddEmployeeUsingSp(AddEmployeeSPCommand command)
        {
            var result = await sender.Send(command);
            return Ok(result);
        }

        [HttpPatch("patch-employee-sp")]
        public async Task<IActionResult> PatchEmployeeUsingSp(PatchEmployeeSpCommand command)
        {
            var result = await sender.Send(command);
            return Ok(result);
        }
        [HttpPut("update-employee-sp")]
        public async Task<IActionResult> UpdateEmployeeUsingSp(UpdateEmployeeSpCommand command)
        {
            var result = await sender.Send(command);
            return Ok(result);
        }
    }
}
