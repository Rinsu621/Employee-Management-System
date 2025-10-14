using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeCRUD.Application.EmployeeModule.Dtos
{
    public class EmployeePdfModel
    {
        public int SNo { get; set; }            // Serial number
        public string Name { get; set; } = "";  // Employee name
        public string Email { get; set; } = ""; // Employee email
        public string Phone { get; set; } = ""; // Phone number
        public string Department { get; set; } = ""; // Department name
        public string Role { get; set; } = "";       // Role or designation
        public DateTime CreatedAt { get; set; }      // Joining date
    }
}
