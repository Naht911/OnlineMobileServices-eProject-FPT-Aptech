using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineMobileServices_Models.DTOs
{
    public class UserLoginDTO
    {
        public required string MobileNumber { get; set; }
        public required string Password { get; set; }
    }
}
