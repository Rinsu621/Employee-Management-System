using EmployeeCRUD.Application.GeminiModule.Queries;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeCRUD.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GeminiController : ControllerBase
    {
        private readonly IMediator mediator;
        public GeminiController(IMediator _mediator)
        {
            mediator = _mediator;
        }

        [HttpPost("generate")]
        public async Task<IActionResult> GenerateResponse(GenerateGeminiResponseQuery query)
        {
            var response = await mediator.Send(query);
            return Ok(response);
        }
    }
}
