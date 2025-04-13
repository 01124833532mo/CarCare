using CareCare.Core.Application.Abstraction.Models.Auth.UserDtos;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CareCare.Core.Application.Abstraction.Validators.Auth
{
	public class TechLocationValidator : AbstractValidator<UpdateTechnicalLocationDto>
	{
		public TechLocationValidator()
		{
			RuleFor(x => x.TechLatitude)
				.InclusiveBetween(22.0, 31.7)
				.WithMessage("Latitude must be between 22.0 and 31.7 to be inside Egypt.");

			RuleFor(x => x.TechLongitude)
				.InclusiveBetween(24.7, 36.9)
				.WithMessage("Longitude must be between 24.7 and 36.9 to be inside Egypt.");
		}
	}
}
