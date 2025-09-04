using EmployeeCRUD.Application.Dtos.Employees;
using EmployeeCRUD.Application.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeCRUD.Application.Queries.Employees
{
   public record GetEmployeeByIdQuery(Guid Id) : IRequest<EmployeeResponseDto>;

    public class GetEmployeeByIdHandler : IRequestHandler<GetEmployeeByIdQuery, EmployeeResponseDto>
    {
        private readonly IEmployeeRepository employeeRepository;
        public GetEmployeeByIdHandler(IEmployeeRepository _employeeRepository)
        {
            employeeRepository = _employeeRepository;
        }
        public async Task<EmployeeResponseDto> Handle(GetEmployeeByIdQuery request, CancellationToken cancellationToken)
        {
            var e = await employeeRepository.GetEmployeeByIdAsync(request.Id);
            if (e == null)
            {
                throw new KeyNotFoundException($"Employee with Id '{request.Id}' not found.");
            }
            return new EmployeeResponseDto
            {
                Id = e.Id,
                Name = e.EmpName,
                Email = e.Email,
                Phone = e.Phone,
                CreatedAt = e.CreatedAt
            };
        }
    }
}
