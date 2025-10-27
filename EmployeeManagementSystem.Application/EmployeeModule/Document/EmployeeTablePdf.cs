using EmployeeManagementSystem.Application.EmployeeModule.Dtos;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagementSystem.Application.EmployeeModule.Document
{
    public class EmployeeTablePdf:IDocument
    {
        private readonly List<EmployeePdfModel> _employees;
        public EmployeeTablePdf(List<EmployeePdfModel> employees)
        {
            _employees = employees;
        }

        public void Compose(IDocumentContainer container)
        {
            container.Page(page =>
            {
                page.Margin(20);
                page.Header().Text("Employee List").FontSize(20).Bold().AlignCenter();
                page.Content().Table(table =>
                {
                    table.ColumnsDefinition(columns =>
                    {
                        for (int i = 0; i < 7; i++) columns.RelativeColumn();
                    });

                    table.Header(header =>
                    {
                        header.Cell().Element(CellStyleHeader).Text("S.No");
                        header.Cell().Element(CellStyleHeader).Text("Name");
                        header.Cell().Element(CellStyleHeader).Text("Email");
                        header.Cell().Element(CellStyleHeader).Text("Phone");
                        header.Cell().Element(CellStyleHeader).Text("Department");
                        header.Cell().Element(CellStyleHeader).Text("Role");
                        header.Cell().Element(CellStyleHeader).Text("Created At");

                        static IContainer CellStyleHeader(IContainer container)
                        {
                            return container.DefaultTextStyle(x => x.SemiBold())
                                .PaddingVertical(5)
                                .BorderBottom(1)
                                .BorderColor(Colors.Black);
                        }
                    });

                    foreach (var emp in _employees)
                    {
                        table.Cell().Element(CellStyleRow).Text(emp.SNo.ToString());
                        table.Cell().Element(CellStyleRow).Text(emp.Name);
                        table.Cell().Element(CellStyleRow).Text(emp.Email);
                        table.Cell().Element(CellStyleRow).Text(emp.Phone);
                        table.Cell().Element(CellStyleRow).Text(emp.Department);
                        table.Cell().Element(CellStyleRow).Text(emp.Role);
                        table.Cell().Element(CellStyleRow).Text(emp.CreatedAt.ToString("yyyy-MM-dd"));

                        static IContainer CellStyleRow(IContainer container)
                        {
                            return container.BorderBottom(1)
                                .BorderColor(Colors.Grey.Lighten2)
                                .PaddingVertical(5);
                        }
                    }
                });
            });
        }
    }
}
