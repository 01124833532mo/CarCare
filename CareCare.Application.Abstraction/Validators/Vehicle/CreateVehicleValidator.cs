using CarCare.Shared.Models._Common;
using CareCare.Core.Application.Abstraction.Models.Vehicles;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CareCare.Core.Application.Abstraction.Validators.Vehicle
{
	public class CreateVehicleValidator : AbstractValidator<CreateVehicleDto>
	{
		public CreateVehicleValidator()
		{

			RuleFor(x => x.Year)
				.NotNull()
				.WithMessage("Year Must Not Null ,Plz Add a {PropertyName}")
				.NotEmpty()
				.WithMessage("Year Must Not Empty ,Plz Add a {PropertyName}")
				.InclusiveBetween(1980, DateTime.Now.Year)
				.WithMessage("Car year must be between 1980 and current year.");

			RuleFor(x => x.Model)
				.NotNull()
				.WithMessage("Model Must Not Null ,Plz Add a {PropertyName}")
				.NotEmpty()
				.WithMessage("Model Must Not Empty ,Plz Add a {PropertyName}");

			RuleFor(x => x.VIN_Number)
				.NotNull()
				.WithMessage("VIN_Number Must Not Null ,Plz Add a {PropertyName}")
				.NotEmpty()
				.WithMessage("VIN_Number Must Not Empty ,Plz Add a {PropertyName}");

			RuleFor(x => x.PlateNumber)
				.NotNull()
				.WithMessage("PlateNumber Must Not Null ,Plz Add a {PropertyName}")
				.NotEmpty()
				.WithMessage("PlateNumber Must Not Empty ,Plz Add a {PropertyName}");

			RuleFor(x => x.Color)
				.NotNull()
				.WithMessage("Color Must Not Null ,Plz Add a {PropertyName}")
				.NotEmpty()
				.WithMessage("Color Must Not Empty ,Plz Add a {PropertyName}");
		}
	}

}
