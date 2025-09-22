using EmployeeCRUD.Application.AuthModel.Commands;
using EmployeeCRUD.Application.Interface;
using EmployeeCRUD.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace EmployeeCRUD.Api.Controllers
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
        [Authorize]
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

    }
}
