using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CareCare.Core.Application.Abstraction.Models.Auth._Common
{
    public class LoginDto
    {

        public required string PhoneNumber { get; set; }


        public required string Password { get; set; }

    }
}
