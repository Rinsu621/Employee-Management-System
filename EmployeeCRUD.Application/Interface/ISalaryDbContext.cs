using EmployeeCRUD.Domain.Entities;
using Microsoft.EntityFrameworkCore;


namespace EmployeeCRUD.Application.Interface
{
    public interface ISalaryDbContext
    {
        DbSet<Salary> Salaries { get; set; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
