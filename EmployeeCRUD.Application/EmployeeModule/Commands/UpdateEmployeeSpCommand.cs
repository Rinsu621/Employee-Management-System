using EmployeeCRUD.Application.Interface;
using EmployeeCRUD.Infrastructure.Data.keyless;
using MediatR;
using Microsoft.EntityFrameworkCore;


namespace EmployeeCRUD.Application.EmployeeModule.Commands
{
    public record UpdateEmployeeSpCommand( Guid Id,  string EmpName, string Email, string Phone) : IRequest<EmployeeUpdateKeyless>;

    public class UpdateEmployeeSpHandler : IRequestHandler<UpdateEmployeeSpCommand, EmployeeUpdateKeyless>
    {
        private readonly IAppDbContext dbContext;

        public UpdateEmployeeSpHandler(IAppDbContext _dbContext)
        {
            dbContext = _dbContext;
        }

        public async Task<EmployeeUpdateKeyless> Handle(UpdateEmployeeSpCommand request, CancellationToken cancellationToken)
        {

            var updatedEmployee = dbContext.EmployeeUpdateKeyless
                .FromSqlInterpolated($"EXEC UpdateEmployee {request.Id}, {request.EmpName}, {request.Email}, {request.Phone}")
                .AsNoTracking()
                .AsEnumerable()
                .FirstOrDefault();

            return updatedEmployee;
        }
    }
  }
