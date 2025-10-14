using EmployeeCRUD.Application.ProjectModule.Commands;
using EmployeeCRUD.Application.ProjectModule.Query;
using MediatR;
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

        //[ServiceFilter(typeof(ProjectTeamMemberFilter))]
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
        //[HttpPatch("assign-department")]
        //public async Task<IActionResult> AssignDepartmentInProject([FromBody] AssignDepartmentInProjectCommand command)
        //{
        //    var result = await sender.Send(command);
        //    return Ok(result);
        //}

        //[HttpPatch("assign-project-manager")]
        //public async Task<IActionResult> AssignProjectManager(AssignProjectManagerCommand command)
        //{
        //    var result = await sender.Send(command);
        //    return Ok(result);
        //}

        //[HttpPatch("update-status")]
        //public async Task<IActionResult> UpdateStatus(UpdateStatusCommand command)
        //{
        //    var result = await sender.Send(command);
        //    return Ok(result);
        //}

        //[HttpPatch("assign-team-member")]
        //public async Task<IActionResult> AssignTeamMember(AssignTeamMemberCommand command)
        //{

        //    var result = await sender.Send(command);
        //    return Ok(result);
        //}

        [HttpPatch("Patch-Project")]
        public async Task<IActionResult> PatchProject(PatchProjectCommand command)
        {
            var result = await sender.Send(command);
            return Ok(result);
        }

        [HttpDelete("delete-project")]
        public async Task<IActionResult> DeleteProject(DeleteProjectCommand command)
        {
            var result = await sender.Send(command);
            return Ok(result);
        }

        //Using SP
        [HttpPost("add-project-sp")]
        public async Task<IActionResult> AddProjectUsingSP(AddProjectSpCommand command)
        {
            var result = await sender.Send(command);
            return Ok(result);
        }

        [HttpPatch("assign-team-member-sp")]
        public async Task<IActionResult> AssignTeamMemberUsingSP(AssignTeamMemberSpCommand command)
        {
            var result = await sender.Send(command);
            return Ok(result);
        }

        [HttpPatch("assign-department-sp")]
        public async Task<IActionResult> AssignDepartmentInProjectUsingSP(AssignDepartmentSpCommand command)
        {
            var result = await sender.Send(command);
            return Ok(result);
        }

        [HttpPost("add-project-dapper")]
        public async Task<IActionResult> AddProjectUsingDapper(AddProjectDapperCommand command)
        {
            var result = await sender.Send(command);
            return Ok(result);
        }

        //using dapper
        [HttpPatch("patch-project-dapper")]
        public async Task<IActionResult> PatchProjectUsingDapper(PatchProjectDapperCommand command)
        {
            var result = await sender.Send(command);
            return Ok(result);
        }


    }
}
