using EmployeeCRUD.Application.Interface;
using Microsoft.Data.SqlClient;
using System.Data;

namespace EmployeeCRUD.Infrastructure.Data
{
    public class DbConnectionService : IDbConnectionService, IEmployeeDbConnection, ISalaryDbConnection
    {
        private readonly string _connectionString;

        public DbConnectionService(string connectionString)
        {
            _connectionString = connectionString;
        }

        public IDbConnection CreateConnection()
        {
            return new SqlConnection(_connectionString);
        }
    }
}