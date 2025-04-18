using CareCare.Core.Application.Abstraction.Models.FeedBack;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CareCare.Core.Application.Abstraction.Validators.Feedback
{
	public class CreateFeedBackValidator : AbstractValidator<CreateFeedBackDto>
	{
		public CreateFeedBackValidator()
		{

			RuleFor(x => x.Rating)
				.NotEmpty()
				.WithMessage("Rating Must Not Empty ,Plz Add a {PropertyName}")
				.LessThanOrEqualTo(5)
				.WithMessage("Rating Must be Less than or Equal to 5")
				.GreaterThanOrEqualTo(0)
				.WithMessage("Rating Must be Greater than or Equal to 0");
		}
	}
}
