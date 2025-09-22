using EmployeeCRUD.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeCRUD.Application.Interface
{
    public interface ISalaryDbContext
    {
        DbSet<Salary> Salaries { get; set; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
