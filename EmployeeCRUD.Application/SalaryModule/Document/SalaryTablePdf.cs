using EmployeeCRUD.Application.SalaryModule.DTO;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace EmployeeCRUD.Application.SalaryModule.Document
{
    public class SalaryTablePdf : IDocument
    {
        private readonly SalaryPdfModel _salary;

        public SalaryTablePdf(SalaryPdfModel salary)
        {
            _salary = salary;
        }

        public void Compose(IDocumentContainer container)
        {
            container.Page(page =>
            {
                page.Margin(30);

                // Header Section
                page.Header().Column(col =>
                {
                    col.Item().Text("My Company Pvt. Ltd.")
                        .FontSize(20).Bold().AlignCenter();
                    col.Item().Text($"Salary Slip for {_salary.SalaryMonth:MM/yyyy}")
                        .FontSize(14).Italic().AlignCenter();
                });

                page.Content().Padding(15).Column(col =>
                {
                    col.Item().Text("Employee Information")
                       .FontSize(14).Bold().FontColor(Colors.Blue.Medium);
                    // Employee Information Table
                    col.Item().Table(table =>
                    {
                        table.ColumnsDefinition(columns =>
                        {
                            columns.ConstantColumn(120);
                            columns.RelativeColumn();
                            columns.ConstantColumn(120);
                            columns.RelativeColumn();
                        });

                        table.Cell().Element(CellStyle).Text("Name:").Bold();
                        table.Cell().Element(CellStyle).Text(_salary.EmployeeName);
                        table.Cell().Element(CellStyle).Text("Department:").Bold();
                        table.Cell().Element(CellStyle).Text(_salary.Department ?? "-");

                        table.Cell().Element(CellStyle).Text("Role:").Bold();
                        table.Cell().Element(CellStyle).Text(_salary.Role ?? "-");
                        table.Cell().Element(CellStyle).Text("Joined:").Bold();
                        table.Cell().Element(CellStyle).Text(_salary.Joined);

                        table.Cell().Element(CellStyle).Text("Payment Method:").Bold();
                        table.Cell().Element(CellStyle).Text(_salary.PaymentMode ?? "-");
                        table.Cell().Element(CellStyle).Text("Status:").Bold();
                        table.Cell().Element(CellStyle).Text(_salary.Status);

                    });

                    col.Item().PaddingVertical(10).LineHorizontal(1).LineColor(Colors.Grey.Lighten2);

                    col.Item().Text("Earnings & Deductions")
                        .FontSize(14).Bold().FontColor(Colors.Blue.Medium);
                    // Earnings and Deductions Table
                    col.Item().Table(table =>
                    {
                        // Earning
                        table.ColumnsDefinition(columns =>
                        {
                            columns.ConstantColumn(35);
                            columns.RelativeColumn(2);
                            columns.ConstantColumn(80);
                            columns.ConstantColumn(35);
                            columns.RelativeColumn(2);
                            columns.ConstantColumn(80);
                        });

                        table.Cell().Element(HeaderCellStyle).Text("S.No").Bold();
                        table.Cell().Element(HeaderCellStyle).Text("Earning").Bold();
                        table.Cell().Element(HeaderCellStyle).Text("Amount").Bold();
                        table.Cell().Element(HeaderCellStyle).Text("S.No").Bold();
                        table.Cell().Element(HeaderCellStyle).Text("Deduction").Bold();
                        table.Cell().Element(HeaderCellStyle).Text("Amount").Bold();

                        table.Cell().Element(CellStyle).Text("1");
                        table.Cell().Element(CellStyle).Text("Basic Salary");
                        table.Cell().Element(CellStyle).Text($"{_salary.BasicSalary:0.00}");
                        table.Cell().Element(CellStyle).Text("1");
                        table.Cell().Element(CellStyle).Text("Professional Tax");
                        table.Cell().Element(CellStyle).Text($"{_salary.Tax:0.00}");

                        // Row 2
                        table.Cell().Element(CellStyle).Text("2");
                        table.Cell().Element(CellStyle).Text("Conveyance");
                        table.Cell().Element(CellStyle).Text($"{_salary.Conveyance:0.00}");
                        table.Cell().Element(CellStyle).Text("2");
                        table.Cell().Element(CellStyle).Text("PF");
                        table.Cell().Element(CellStyle).Text($"{_salary.PF:0.00}");

                        // Row 3
                        table.Cell().Element(CellStyle).Text("");
                        table.Cell().Element(CellStyle).Text("Gross Salary").Bold();
                        table.Cell().Element(CellStyle).Text($"{_salary.GrossSalary:0.00}").Bold();
                        table.Cell().Element(CellStyle).Text("3");
                        table.Cell().Element(CellStyle).Text("ESI");
                        table.Cell().Element(CellStyle).Text($"{_salary.ESI:0.00}");

                        // Row 4 (Total Deduction)
                        table.Cell().Element(CellStyle).Text("");
                        table.Cell().Element(CellStyle).Text("");
                        table.Cell().Element(CellStyle).Text("");
                        table.Cell().Element(CellStyle).Text("");
                        table.Cell().Element(CellStyle).Text("Total Deduction").Bold();
                        table.Cell().Element(CellStyle).Text($"{_salary.Tax + _salary.PF + _salary.ESI:0.00}").Bold();
                    });

                    col.Item().PaddingVertical(10).Text("Net Salary")
                      .FontSize(14).Bold().FontColor(Colors.Blue.Medium);

                    // Net Salary Table
                    col.Item().Table(table =>
                    {
                        table.ColumnsDefinition(columns =>
                        {
                            columns.RelativeColumn();
                            columns.ConstantColumn(100);
                        });

                        table.Cell().Element(CellStyle).Text("Net Salary (NPR)").Bold();
                        table.Cell().Element(CellStyle).Text($"{_salary.NetSalary:0.00}").Bold();
                    });
                });
                page.Footer().AlignCenter().Text("This is a computer-generated salary slip. Please do not reply.")
                   .FontSize(10).Italic().FontColor(Colors.Grey.Darken2);
            });
        }

        private IContainer CellStyle(IContainer container)
        {
            return container.Padding(4)
                .BorderBottom(1)
                .BorderColor(Colors.Grey.Lighten2);
        }

        private IContainer HeaderCellStyle(IContainer container)
        {
            return container
                .Padding(4)
                .Background(Colors.Grey.Lighten3)
                .BorderBottom(1)
                .BorderColor(Colors.Grey.Lighten1);
        }
    }
}

     