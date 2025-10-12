using EmployeeCRUD.Application.EmployeeModule.Dtos;
using EmployeeCRUD.Application.Interface;
using EmployeeCRUD.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Tables;
using MigraDoc.DocumentObjectModel.Shapes;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace EmployeeCRUD.Application.EmployeeModule.Queries
{
    public record ExportEmployeeProfileQuery(Guid EmployeeId) : IRequest<byte[]>;

    public class ExportEmployeeProfileHandler : IRequestHandler<ExportEmployeeProfileQuery, byte[]>
    {
        private readonly IAppDbContext dbContext;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IPdfService pdfService;

        public ExportEmployeeProfileHandler(IAppDbContext _dbContext, UserManager<ApplicationUser> _userManager, IPdfService _pdfService)
        {
            dbContext = _dbContext;
            userManager = _userManager;
            pdfService = _pdfService;
        }

        public async Task<byte[]> Handle(ExportEmployeeProfileQuery request, CancellationToken cancellationToken)
        {
            var employee = await dbContext.Employees
                .Include(e => e.Department)
                .FirstOrDefaultAsync(e => e.Id == request.EmployeeId, cancellationToken);
            if (employee == null)
                throw new Exception("Employee not found");


            var user = await userManager.Users.FirstOrDefaultAsync(u => u.Email == employee.Email, cancellationToken);
           
            var role = await userManager.GetRolesAsync(user);

            // Use default avatar if employee.Avatar is null
            string avatarPath = @"D:\Project_Intern\EmployeeCRUD.Api\employeecrud.view\public\OIP.jpeg";


            var pdfDetail = new EmployeeProfilePdfModelDto
            {
                EmpName = employee.EmpName,
                Email = employee.Email,
                Phone = employee.Phone,
                Role = role.FirstOrDefault() ?? "N/A",
                Department = employee.Department?.DeptName ?? "N/A",
                JoinedDate = employee.CreatedAt,
                Avatar = avatarPath // Use file path instead of base64
            };

            return pdfService.GeneratePdf(pdfDetail, (doc, u) =>
            {
                var section = doc.AddSection();

                // Add Avatar
                if (!string.IsNullOrEmpty(u.Avatar) && File.Exists(u.Avatar))
                {
                    var image = section.AddImage(u.Avatar);
                    image.Width = "2in";
                    image.Height = "2in";
                    image.LockAspectRatio = true;
                    image.Top = ShapePosition.Top;
                    image.Left = ShapePosition.Center;
                    image.WrapFormat.Style = WrapStyle.Through;
                }

                // Set font to Arial explicitly
                var namePara = section.AddParagraph(u.EmpName);
                namePara.Format.Font.Name = "Arial";
                namePara.Format.Font.Bold = true;

                var rolePara = section.AddParagraph(u.Role);
                rolePara.Format.Font.Name = "Arial";
                rolePara.Format.Font.Color = Colors.DarkBlue;

                section.AddParagraph().AddLineBreak();

                // Table with details
                Table table = section.AddTable();
                table.Borders.Width = 0.75;
                table.AddColumn("4cm");
                table.AddColumn("10cm");

                void AddRow(string label, string value)
                {
                    var row = table.AddRow();
                    row.Cells[0].AddParagraph(label).Format.Font.Name = "Arial";
                    row.Cells[1].AddParagraph(value ?? "N/A").Format.Font.Name = "Arial";
                }

                AddRow("Department", u.Department);
                AddRow("Phone", u.Phone);
                AddRow("Email", u.Email);
                AddRow("Joined", u.JoinedDate.ToShortDateString());
            });

        }
    }
}
