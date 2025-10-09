using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeCRUD.Domain.Entities
{
    //By inheriting from IdentityUser, ApplicationUser gets all the properties of IdentityUser like UserName, PasswordHash, Email,roles and claims etc.
    public class ApplicationUser:IdentityUser
    {
        public Guid EmployeeId { get; set; }
        public Employee Employee { get; set; } //To navigate from Identity user to Employee entity

        //Stores Refresh token for user
        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiryTime { get; set; }


    }
}
