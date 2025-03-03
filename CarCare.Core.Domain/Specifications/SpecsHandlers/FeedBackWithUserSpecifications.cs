using CarCare.Core.Domain.Entities.FeedBacks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarCare.Core.Domain.Specifications.SpecsHandlers
{
	public class FeedBackWithUserSpecifications : BaseSpecification<FeedBack, int>
	{

		public FeedBackWithUserSpecifications(string? Sort)
		{
			switch (Sort)
			{
				case "RateDesc":
					AddOrderByDesc(f => f.Rating);
					break;
				case "RateAsc":
					AddOrderBy(f => f.Rating);
					break;
				default:
					AddOrderByDesc(f => f.CreatedOn);
					break;
			}

		}


		public FeedBackWithUserSpecifications(int id)
			: base(id)
		{
			AddIncludes();
		}

		private protected override void AddIncludes()
		{
			base.AddIncludes();

			Includes.Add(f => f.User);
		}

	}
}
