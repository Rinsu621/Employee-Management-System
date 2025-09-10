using Ardalis.GuardClauses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeCRUD.Domain.Guards
{
    public static class EmployeeGuard
    {
        public static void Validate(string empName, string email, string phone)
        {
            Guard.Against.NullOrEmpty(empName, nameof(empName));
            Guard.Against.InvalidInput(email, nameof(email), e => e.Contains("gmail.com"), "Email must be a Gmail address");
            Guard.Against.InvalidInput(phone, nameof(phone), p => p.StartsWith("98") && p.Length == 10);
        }
    }
}
