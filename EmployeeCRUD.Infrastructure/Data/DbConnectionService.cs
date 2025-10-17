using EmployeeCRUD.Application.Configuration;
using EmployeeCRUD.Application.Interface;
using EmployeeCRUD.Infrastructure.Configurations;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using System.Data;

namespace EmployeeCRUD.Infrastructure.Data
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