using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeCRUD.Application.AuthModel.Dto
{
    public class LoginResponseDto
    {
        public string Token { get; set; }

        public string RefreshToken { get; set; }

        public string Name { get; set; }
        public DateTime Expiration { get; set; }
    }
}
