using EmployeeCRUD.Application.Dtos.Employees;
using EmployeeCRUD.Application.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeCRUD.Application.Command.Employees
{
  public record DeleteEmployeeCommand(Guid Id):IRequest<DeleteEmployeeResponse>;
    public class DeleteEmployeeHandler : IRequestHandler<DeleteEmployeeCommand, DeleteEmployeeResponse>
    {
        private readonly IEmployeeRepository employeeRepository;
        public DeleteEmployeeHandler(IEmployeeRepository _employeeRepository)
        {
            employeeRepository = _employeeRepository;
        }
        public async Task<DeleteEmployeeResponse> Handle(DeleteEmployeeCommand request, CancellationToken cancellationToken)
        {
            var existingEmployee = await employeeRepository.GetEmployeeByIdAsync(request.Id);
            if (existingEmployee == null)
            {
                throw new KeyNotFoundException($"Employee with Id '{request.Id}' not found.");
            }
            await employeeRepository.DeleteEmployeeAsync(existingEmployee);

            return new DeleteEmployeeResponse
            {
                Success = true,
                Message = "Employee removed successfully."
            };
        }

      
    }
}
