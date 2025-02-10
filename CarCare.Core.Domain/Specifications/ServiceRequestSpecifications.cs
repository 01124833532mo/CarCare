using CarCare.Core.Domain.Entities.Identity;
using CarCare.Core.Domain.Entities.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarCare.Core.Domain.Specifications
{
	public class ServiceRequestSpecifications : BaseSpecification<ServiceRequest, int>
	{

		public ServiceRequestSpecifications(Types type, string serviceType)
			: base(request => request.Technical.Type == type && request.ServiceType.Name == serviceType)
		{
			AddIncludes();
		}
		private protected override void AddIncludes()
		{
			base.AddIncludes();
			Includes.Add(r => r.User);
			Includes.Add(r => r.Technical);
			Includes.Add(r => r.ServiceType);
		}
	}
}
