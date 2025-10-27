using EmployeeManagementSystem.Application.SalaryModule.Command;
using EmployeeManagementSystem.Application.SalaryModule.Command.AddSalary;
using EmployeeManagementSystem.Application.SalaryModule.Command.AddSalaryDapper;
using EmployeeManagementSystem.Application.SalaryModule.Queries.GetSalaryDapper;
using EmployeeManagementSystem.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagementSystem.Api.Controllers
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
        public async Task<IActionResult> AddSalary([FromBody] AddSalaryCommand command)
        {
            var result = await mediator.Send(command);
            return Ok(result);
        }

        [HttpPost("add-salary-dapper")]
        public async Task<IActionResult> AddSalaryDapper([FromBody] AddSalaryDapperCommand command)
        {
            var result = await mediator.Send(command);
            return Ok(result);
        }

        [HttpPost("generate-pdf/{salaryId:guid}")]
        public async Task<IActionResult> GenerateSalaryPdf(Guid salaryId)
        {
            var pdfBytes = await mediator.Send(new GenerateSalaryPdfCommand(salaryId));
            return File(pdfBytes, "application/pdf", "SalarySlip.pdf");
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

        [HttpGet("salary-result")]
        public async Task<IActionResult> GetSalaryResult()
        {
            var result = await mediator.Send(new GetSalaryDetailQuery());
            return Ok(result);
        }
    }
}
