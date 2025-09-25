using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeCRUD.Application.EmployeeModule.Dtos
{
    public class EmployeePagedResponseDto
    {
        public IEnumerable<EmployeeResponseDto> Items { get; set; }
        public int TotalCount { get; set; }
    }
}
