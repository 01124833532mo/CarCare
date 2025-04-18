using CarCare.Core.Domain.Common;
using CarCare.Core.Domain.Entities.Common;
using CarCare.Core.Domain.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarCare.Core.Domain.Entities.Contacts
{
	public class Contact : BaseAuditableEntity<int>, IBaseUserId
	{
		public required string Message { get; set; }

		public required Types MessageFor { get; set; }


		public required string UserId { get; set; }
		public virtual required ApplicationUser User { get; set; }


	}
}
