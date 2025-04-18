using CarCare.Shared.Models._Common;
using CareCare.Core.Application.Abstraction.Models.ServiceTypes;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CareCare.Core.Application.Abstraction.Validators.ServiceType
{
	public class ServiceTypeValidator : AbstractValidator<ServiceTypeDto>
	{
		public ServiceTypeValidator()
		{
			RuleFor(x => x.Name)
				.NotEmpty()
				.WithMessage("Name Must Not Empty ,Plz Add a {PropertyName}");

			RuleFor(x => x.Description)
				.NotEmpty()
				.WithMessage("Description Must Not Empty ,Plz Add a {PropertyName}");


		}
	}
}
