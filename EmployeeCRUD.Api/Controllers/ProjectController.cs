using EmployeeCRUD.Application.Command.Projects;
using EmployeeCRUD.Application.Queries.Projects;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeCRUD.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectController(ISender sender) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> AddProject(AddProjectCommand command)
        {
            var result = await sender.Send(command);
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetProjects()
        {
            var result = await sender.Send(new GetProjectQuery());
            return Ok(result);
        }

        [HttpGet("get-by-id")]
        public async Task<IActionResult> GetProjectById(Guid id)
        { 
        var result = await sender.Send(new GetProjectByIdQuery(id));
            return Ok(result);
        }
        [HttpPatch("assign-department")]
        public async Task<IActionResult> AssignDepartmentInProject([FromBody] AssignDepartmentInProjectCommand command)
        {
            var result = await sender.Send(command);
            return Ok(result);
        }


    }
}
