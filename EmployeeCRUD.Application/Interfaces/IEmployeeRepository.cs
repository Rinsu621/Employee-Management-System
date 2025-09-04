using EmployeeCRUD.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeCRUD.Application.Interfaces
{
    public interface IEmployeeRepository
    {
        Task<Employee> AddEmployeeAsync(Employee employee);
        Task<Employee?> GetEmployeeByEmail(string email);
        Task<IEnumerable<Employee>> GetAllEmployeesAsync();

        Task<Employee?> GetEmployeeByIdAsync(Guid id);
        Task<Employee> UpdateEmployeesAsync(Employee employee);
        Task<bool> DeleteEmployeeAsync(Employee employee);

    }
}
