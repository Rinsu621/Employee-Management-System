using EmployeeCRUD.Application.Command.Departments;
using EmployeeCRUD.Application.Dtos.Departments;
using EmployeeCRUD.Application.Queries.Departments;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeCRUD.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentController(ISender sender) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> AddDepartmentAsync([FromBody] DepartmentCreateDto department)
        {
            var result = await sender.Send(new AddDepartmentCommand(department));
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetDepartmentsAsync()
        {
            var result = await sender.Send(new GetDepartmentQuery());
            return Ok(result);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetDepartmentByIdAsync(Guid id)
        {
            var result = await sender.Send(new GetDepartmentByIdQuery(id));
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDepartmentAsync(Guid id, [FromBody] DepartmentCreateDto department)
        {
            var result = await sender.Send(new UpdateDepartmentCommand(id, department));
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDepartmentAsync(Guid id)
        {
            var result = await sender.Send(new DeleteDepartmentCommand(id));
            return Ok(result);
        }

        [HttpPost("add-employee/{departmentId}/{employeeId}")]
        public async Task<IActionResult> AddEmployeeToDepartmentAsync(Guid departmentId, Guid employeeId)
        {
            var result = await sender.Send(new AddEmployeeToDepartmentCommand(departmentId, employeeId));
            return Ok(result);
        }
    }
}
