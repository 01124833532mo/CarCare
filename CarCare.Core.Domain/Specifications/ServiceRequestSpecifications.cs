using CarCare.Core.Domain.Entities.Identity;
using CarCare.Core.Domain.Entities.Orders;
using CarCare.Core.Domain.Entities.ServiceTypes;
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

		public ServiceRequestSpecifications(string userId, int pageSize, int pageIndex)
			: base(request => request.UserId == userId)
		{
			AddIncludes();

			AddOrderByDesc(p => p.CreatedOn);

			ApplyPagination((pageIndex - 1) * pageSize, pageSize);

		}

		public ServiceRequestSpecifications(BusnissStatus? status)
			: base(s => status == null || s.BusnissStatus == status)
		{

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
