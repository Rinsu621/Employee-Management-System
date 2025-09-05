using EmployeeCRUD.Application.Dtos.Employees;
using EmployeeCRUD.Domain.Entities;
using EmployeeCRUD.Infrastructure.Data;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeCRUD.Application.Queries.Employees
{
    public record GetAllEmployeesSPQuery() : IRequest<IEnumerable<EmployeeResponseDto>>;


    public class GetAllEmployeesSPHandler : IRequestHandler<GetAllEmployeesSPQuery, IEnumerable<EmployeeResponseDto>>
    {
        private readonly AppDbContext dbContext;
        public GetAllEmployeesSPHandler(AppDbContext _dbContext)
        {
            dbContext = _dbContext;
        }
        public async Task<IEnumerable<EmployeeResponseDto>> Handle(GetAllEmployeesSPQuery request, CancellationToken cancellationToken)
        {
            var employees = new List<EmployeeResponseDto>();
            var connection = dbContext.Database.GetDbConnection();
            await using (connection)
            {
                await connection.OpenAsync(cancellationToken);
                await using var command = connection.CreateCommand();
                command.CommandText = "GetAllEmployees";
                command.CommandType = System.Data.CommandType.StoredProcedure;

                await using var reader = await command.ExecuteReaderAsync(cancellationToken);
                while (await reader.ReadAsync(cancellationToken))
                {
                    employees.Add(new EmployeeResponseDto
                    {
                        Id = reader.GetGuid(reader.GetOrdinal("Id")),
                        Name = reader.GetString(reader.GetOrdinal("EmpName")),
                        Email = reader.GetString(reader.GetOrdinal("Email")),
                        Phone = reader.GetString(reader.GetOrdinal("Phone")),
                        CreatedAt = reader.GetDateTime(reader.GetOrdinal("CreatedAt"))
                    });
                }
                return employees;
            }


            //use if you use entities instead of Dto
            //var employees = await dbContext.Employees.FromSqlRaw("EXEC GetAllEmployees").ToListAsync(cancellationToken);

            //var employeesDtos = employees.Select(e => new EmployeeResponseDto
            //{
            //    Id = e.Id,
            //    Name = e.EmpName,
            //    Email = e.Email,
            //    Phone = e.Phone,
            //    CreatedAt = e.CreatedAt
            //}).ToList();
            //return employeesDtos;

            //var employees = await dbContext.Employees.FromSql($"EXECUTE GetAllEmployees").ToListAsync(cancellationToken);
            //var result = employees.Select(e => new Employee
            //{
            //    Id = e.Id,
            //    EmpName = e.EmpName,
            //    Email = e.Email,
            //    Phone = e.Phone,
            //    CreatedAt = e.CreatedAt
            //}).ToList();

        }
    }
}
