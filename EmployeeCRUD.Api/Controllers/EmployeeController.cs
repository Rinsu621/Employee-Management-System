
using EmployeeCRUD.Application.EmployeeModule.Commands;
using EmployeeCRUD.Application.EmployeeModule.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
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
        //[HttpGet("getallemployee")]
        //public async Task<IActionResult> GetAllEmployeesAsync([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 5)
        //{
        //    var result = await sender.Send(new GetAllEmployeesQuery(pageNumber, pageSize));
        //    return Ok(result);
        //}

        [HttpGet("getallemployee")]
        public async Task<IActionResult> GetAllEmployees(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? role = null,
        [FromQuery] Guid? departmentId = null,
        [FromQuery] DateTime? fromDate = null,
        [FromQuery] DateTime? toDate = null)
        {
            var query = new GetAllEmployeesQuery(page, pageSize, role, departmentId, fromDate, toDate);
            var result = await sender.Send(query);
            return Ok(result);
        }
        [HttpPost("get-by-email")]
        public async Task<IActionResult> GetEmployeeByEmailAsync(GetEmployeeByEmail query)
        {
            var result = await sender.Send(query);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetEmployeeByIdAsync(Guid id)
        {
            var result = await sender.Send(new GetEmployeeByIdQuery(id));  
            return Ok(result);
        }
      
        [HttpPut("update")]
        public async Task<IActionResult> UpdateEmployeeAsync(UpdateEmployeeCommand command)
        {
            var result = await sender.Send(command);
            return Ok(result);
        }

        [HttpPatch("patch")]
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

        [HttpPost("add-employee-using-dapper")]
        public async Task<IActionResult> AddEmployeeUsingDapper(AddEmployeeDapperCommand command)
        {
            var result = await sender.Send(command);
            return Ok(result);
        }

        [HttpDelete("delete-employee-using-dapper/{id}")]
        public async Task<IActionResult> DeleteEmployeeUsingDapper(Guid id)
        {
            var result = await sender.Send(new DeleteEmployeeDapperCommand(id));
            return Ok(result);
        }

        [HttpPatch("patch-employee-using-dapper")]
        public async Task<IActionResult> PatchEmployeeUsingDapper(PatchEmployeeDapperCommand command)
        {
            var result = await sender.Send(command);
            return Ok(result);
        }

        [HttpGet("get-all-employees-using-dapper")]
        public async Task<IActionResult> GetAllEmployeesUsingDapper()
        {
            var result =  await sender.Send(new GetAllEmployeeDapperQuery());
            return Ok(result);
        }

        [HttpGet("get-employee-by-id-using-dapper/{id}")]
        public async Task<IActionResult> GetEmployeeByIdUsingDapper(Guid id)
        {
            var result = await sender.Send(new GetEmployeeByIdDapperQuery(id));
            return Ok(result);
        }

        [HttpPut("update-using-dapper")]
        public async Task<IActionResult> UpdateEmployeeDapper(UpdateEmployeeWithDapperCommand command)
        {
            var result = await sender.Send(command);
            return Ok(result);
        }
    }
}
