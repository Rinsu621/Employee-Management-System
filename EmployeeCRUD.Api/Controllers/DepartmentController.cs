using EmployeeCRUD.Application.Department.Command;
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
        public async Task<IActionResult> AddDepartmentAsync(AddDepartmentCommand command)
        {
            var result = await sender.Send(command);
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

        [HttpPut("/update-department")]
        public async Task<IActionResult> UpdateDepartmentAsync(UpdateDepartmentCommand command)
        {
            var result = await sender.Send(command);
            return Ok(result);
        }

        [HttpDelete("/delete/{id}")]
        public async Task<IActionResult> DeleteDepartmentAsync(Guid id)
        {
            var result = await sender.Send(new DeleteDepartmentCommand(id));
            return Ok(result);
        }

        [HttpPost("add-employee")]
        public async Task<IActionResult> AddEmployeeToDepartmentAsync(AddEmployeeToDepartmentCommand command)
        {
            var result = await sender.Send(command);
            return Ok(result);
        }
    }
}
