using EmployeeCRUD.Application.Interface;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeCRUD.Application.EmployeeModule.Queries
{
    public record ExportEmployeeTableQuery(string UserTableHtml) : IRequest<byte[]>;

    public class ExportEmployeeTableHandler : IRequestHandler<ExportEmployeeTableQuery, byte[]>
    {
        private readonly IPdfService pdfService;

        public ExportEmployeeTableHandler(IPdfService _pdfService)
        {
            pdfService = _pdfService;
        }

        public async Task<byte[]> Handle(ExportEmployeeTableQuery request, CancellationToken cancellationToken)
        {
            string tableHtml = request.UserTableHtml;
            var pdfBytes = await pdfService.GeneratePdfAsync(tableHtml);
            return pdfBytes;
        }
    }
}
