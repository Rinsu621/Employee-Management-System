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
    public record ExportEmployeeProfileQuery(Guid EmployeeId):IRequest<byte[]>;
    
    public class ExportEmployeeProfileHandler : IRequestHandler<ExportEmployeeProfileQuery, byte[]>
    {
        private readonly IAppDbContext dbContext;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IPdfService pdfService;

        public ExportEmployeeProfileHandler(
        IAppDbContext _dbContext,
        UserManager<ApplicationUser> _userManager,
        IPdfService _pdfService)
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
            string? role = null;
            if (user != null)
            {
                var roles = await userManager.GetRolesAsync(user);
                role = roles.FirstOrDefault();
            }

            string avatarBase64 = Convert.ToBase64String(File.ReadAllBytes(@"D:\Project_Intern\EmployeeCRUD.Api\employeecrud.view\public\OIP.jpeg"));


            var pdfModel = new EmployeeProfilePdfModelDto
            {
                EmpName = employee.EmpName,
                Email = employee.Email,
                Phone = employee.Phone,
                Role = role ?? "N/A",
                Department = employee.Department?.DeptName,
                JoinedDate = employee.CreatedAt,
                AvatarBase64 = avatarBase64
            };

            // Generate PDF using service
            return await pdfService.ExportAsPdf(pdfModel);
        }
    }
}
