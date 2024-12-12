using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CareCare.Core.Application.Abstraction.Models.Auth
{
    public class RoleDto
    {
        public  string Id { get; set; }

        public required string Name { get; set; }

        public RoleDto()
        {
            Id = Guid.NewGuid().ToString();
        }
    }
}
