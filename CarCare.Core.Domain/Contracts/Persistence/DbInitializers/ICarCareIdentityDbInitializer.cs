using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarCare.Core.Domain.Contracts.Persistence.DbInitializers
{
	public interface ICarCareIdentityDbInitializer
	{
		Task InitializeAsync();
		Task SeedAsync();
	}
}
