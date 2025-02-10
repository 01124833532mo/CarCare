using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarCare.Core.Domain.Entities.Orders
{
	public enum Status
	{
		Pending = 1,
		InProgress,
		Completed,
		Canceled,
	}
}
