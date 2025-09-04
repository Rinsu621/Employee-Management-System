using EmployeeCRUD.Application.Command.Departments;
using EmployeeCRUD.Application.Dtos.Departments;
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
    }
}
