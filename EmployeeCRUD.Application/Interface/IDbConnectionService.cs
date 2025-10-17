using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeCRUD.Application.Interface
{
    public interface IDbConnectionService
    {
        IDbConnection CreateConnection(string? connectionString = null);
    }
}
