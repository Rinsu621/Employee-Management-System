using EmployeeManagementSystem.Application.EmployeeModule.Dtos;
using EmployeeManagementSystem.Application.Interface;
using EmployeeManagementSystem.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagementSystem.Application.EmployeeModule.Queries
{
    public record ExportEmployeeProfileQuery(Guid EmployeeId):IRequest<byte[]>;
    
}
