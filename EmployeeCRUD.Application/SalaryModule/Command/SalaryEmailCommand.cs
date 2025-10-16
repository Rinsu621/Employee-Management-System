using EmployeeCRUD.Application.Interface;
using EmployeeCRUD.Application.SalaryModule.DTO;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeCRUD.Application.SalaryModule.Command
{
    public record SalaryEmailCommand(string Email, byte[] PdfBytes, string FileName, SalaryPdfModel SalaryModel):IRequest;

    public class SalaryEmailHandler : IRequestHandler<SalaryEmailCommand>
    {
        private readonly ISalaryEmailService salaryEmailService;

        public SalaryEmailHandler(ISalaryEmailService _salaryEmailService)
        {
            salaryEmailService = _salaryEmailService;
        }

        public async Task Handle(SalaryEmailCommand request, CancellationToken cancellationToken)
        {
            await salaryEmailService.SendSalarySlipAsync(request.Email, request.PdfBytes, request.FileName, request.SalaryModel);
        }
    }
}
