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
				.NotNull()
				.WithMessage("Latitude Must Not Null , Plz Add a {PropertyName}")
				.NotEmpty()
				.WithMessage("Latitude Must Not Empty , Plz Add a {PropertyName}")
				.InclusiveBetween(22.0, 31.7)
				.WithMessage("Latitude must be between 22.0 and 31.7 to be inside Egypt.");

			RuleFor(x => x.TechLongitude)
				.NotNull()
				.WithMessage("Longitude Must Not Null , Plz Add a {PropertyName}")
				.NotEmpty()
				.WithMessage("Longitude Must Not Empty , Plz Add a {PropertyName}")
				.InclusiveBetween(24.7, 36.9)
				.WithMessage("Longitude must be between 24.7 and 36.9 to be inside Egypt.");
		}
	}
}
