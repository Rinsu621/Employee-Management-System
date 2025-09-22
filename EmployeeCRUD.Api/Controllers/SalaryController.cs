using EmployeeCRUD.Application.SalaryModule.Command;
using EmployeeCRUD.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeCRUD.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SalaryController : ControllerBase
    {
        private readonly IMediator mediator;
        public SalaryController(IMediator _mediator)
        {
            mediator = _mediator;
        }

        [HttpPost("add-salary")]
        public async Task<IActionResult> AddSalary(AddSalaryCommand command)
        {
            var result= await mediator.Send(command);
            return Ok(result);
        }
        [HttpGet("payment-mode")]
        public IActionResult GetPaymentMethods()
        {
            var paymentMethods = Enum.GetNames(typeof(PaymentMethod));
            return Ok(paymentMethods);
        }

        [HttpGet("salary-status")]
        public IActionResult GetSalaryStatus()
        {
            var salaryStatus = Enum.GetNames(typeof(SalaryStatus));
            return Ok(salaryStatus);
        }
    }
}
