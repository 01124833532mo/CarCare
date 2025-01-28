using CarCare.Core.Domain.Entities.Vehicles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarCare.Core.Domain.Specifications.SpecsHandlers
{
	public class VehicleWithUserSpecifications : BaseSpecification<Vehicle, int>
	{	

		public VehicleWithUserSpecifications(int id)
			:base(id) 
		{
			AddIncludes();
		}

		private protected override void AddIncludes()
		{
			base.AddIncludes();

			Includes.Add(v => v.User);

		}
	}
}
