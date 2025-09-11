using EmployeeCRUD.Domain.Entities;
using EmployeeCRUD.Infrastructure.Data.keyless;
using EmployeeCRUD.Infrastructure.Data.Keyless;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeCRUD.Application.Interface
{
    public interface IAppDbContext
    {
        DbSet<Employee> Employees { get; set; }
        DbSet<Department> Departments { get; set; }
        DbSet<Project> Projects { get; set; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

    }
}
