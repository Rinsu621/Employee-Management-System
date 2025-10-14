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

                // Header
                page.Header()
                    .Text("Salary Slip")
                    .FontSize(24)
                    .Bold()
                    .AlignCenter();


                // Content
                page.Content()
                    .Padding(20)
                    .Column(column =>
                    {
                        
                        column.Item().Text($"Employee: {_salary.EmployeeName}")
                              .FontSize(16)
                              .Bold();

                        

                        column.Item().PaddingVertical(10).LineHorizontal(1).LineColor(Colors.Grey.Lighten2);

                        
                        column.Item().Row(row =>
                        {
                           
                            row.RelativeItem().Column(earn =>
                            {
                                earn.Item().Text("Earnings").Bold().FontSize(14).Underline();
                                earn.Item().Text($"Basic Salary: {_salary.BasicSalary:0.00}");
                                earn.Item().Text($"Conveyance: {_salary.Conveyance:0.00}");
                                earn.Item().Text($"Gross Salary: {_salary.GrossSalary:0.00}").Bold();
                            });

                            
                            row.RelativeItem().Column(ded =>
                            {
                                ded.Item().Text("Deductions").Bold().FontSize(14).Underline();
                                ded.Item().Text($"Tax: {_salary.Tax:0.00}");
                                ded.Item().Text($"PF: {_salary.PF:0.00}");
                                ded.Item().Text($"ESI: {_salary.ESI:0.00}");
                                ded.Item().Text($"Net Salary: {_salary.NetSalary:0.00}").Bold();
                            });
                        });

                        column.Item().PaddingVertical(10).LineHorizontal(1).LineColor(Colors.Grey.Lighten2);

                        
                        column.Item().Text($"Payment Mode: {_salary.PaymentMode}").FontSize(12).Bold();
                        column.Item().Text($"Status: {_salary.Status}").FontSize(12).Bold();
                    });

                // Footer
                page.Footer()
                    .AlignCenter()
                    .Text("This is a computer-generated salary slip.")
                    .FontSize(10)
                    .Italic()
                    .FontColor(Colors.Grey.Darken1);
            });
        }
    }
}
