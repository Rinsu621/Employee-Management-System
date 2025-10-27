using EmployeeManagementSystem.Application.Configuration;
using EmployeeManagementSystem.Application.Interface;
using EmployeeManagementSystem.Infrastructure.Configurations;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using System.Data;

namespace EmployeeManagementSystem.Infrastructure.Data
{
    public class DbConnectionService : IDbConnectionService
    {
        private readonly DbSettings _dbSettings;

        public DbConnectionService(IOptions<DbSettings> options)
        {
            _dbSettings = options.Value;
        }

        public IDbConnection CreateConnection(string? connectionString = null)
        {

            var connStr = connectionString ?? _dbSettings.DefaultConnection;
            return new SqlConnection(connStr);
        }
    }
}