using EmployeeManagementSystem.Application.AuthModel.Commands;
using EmployeeManagementSystem.Application.AuthModel.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;


namespace EmployeeManagementSystem.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {

        private readonly IMediator mediator;

        public AuthController(IMediator _mediator)
        {
            mediator = _mediator;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginCommand command)
        {
            var result = await mediator.Send(command);
            return Ok(result);
        }

        [HttpPost("change-password")]
     
        public async Task<IActionResult> ChangePassword(ChangePasswordCommand command)
        {

            var result = await mediator.Send(command);

            return Ok(result);
        }

        [HttpPost("refresh-token")]

        public async Task<IActionResult> RefreshToken(RefreshTokenCommand command)
        {
            var result= await mediator.Send(command);
            return Ok(result);
        }

        [HttpGet("roles")]
       public async Task<IActionResult> GetRoles()
        {
            var roles = await mediator.Send(new GetRolesQuery());
            return Ok(roles);
        }

    }
}
