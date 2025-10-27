using EmployeeManagementSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;


namespace EmployeeManagementSystem.Application.Interface
{
    public interface ISalaryDbContext
    {
        DbSet<Salary> Salaries { get; set; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
