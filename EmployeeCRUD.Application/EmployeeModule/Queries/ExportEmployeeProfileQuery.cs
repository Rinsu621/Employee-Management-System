using EmployeeCRUD.Application.EmployeeModule.Dtos;
using EmployeeCRUD.Application.Interface;
using EmployeeCRUD.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeCRUD.Application.EmployeeModule.Queries
{
    public record ExportEmployeeProfileQuery(Guid EmployeeId):IRequest<byte[]>;
    
}
