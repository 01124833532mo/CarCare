using CarCare.Core.Domain.Entities.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarCare.Core.Domain.Specifications
{
	public class ServiceRequestSpecificationsCount : BaseSpecification<ServiceRequest, int>
	{
		public ServiceRequestSpecificationsCount(string userId)
			: base(request => request.UserId == userId)
		{

		}
	}
}
