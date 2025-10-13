using EmployeeCRUD.Application.EmployeeModule.Dtos;
using EmployeeCRUD.Application.Interface;
using EmployeeCRUD.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeCRUD.Application.EmployeeModule.Queries
{
    public record ExportEmployeeProfileQuery(string ProfileCardHtml) : IRequest<byte[]>;

    public class ExportEmployeeProfileHandler : IRequestHandler<ExportEmployeeProfileQuery, byte[]>
    {
        private readonly IPdfService pdfService;

        public ExportEmployeeProfileHandler(IPdfService _pdfService)
        {
            pdfService = _pdfService;
        }
        public async Task<byte[]> Handle(ExportEmployeeProfileQuery request, CancellationToken cancellationToken)
        {
            string profileHtml = request.ProfileCardHtml;
            var pdfBytes = await pdfService.GeneratePdfAsync(profileHtml);
            return pdfBytes;

        }
    }
}
       