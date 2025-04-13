using CarCare.Shared.Models._Common;
using CareCare.Core.Application.Abstraction.Models.Auth._Common;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CareCare.Core.Application.Abstraction.Validators.Auth
{
	public class LoginValidator : AbstractValidator<LoginDto>
	{
		public LoginValidator()
		{
			RuleFor(x => x.PhoneNumber)
				.NotEmpty()
				.WithMessage("PhoneNumber Must Not Empty , Plz Add a {PropertyName}")
				.Matches(RegexPatterns.PhoneNumber).WithMessage("Invalid Egyptian phone number.");

			RuleFor(x => x.Password)
							.NotEmpty()
							.WithMessage("\"Password Must Not Empty ,Plz Add a {PropertyName}\"")
							.Matches(RegexPatterns.Password).WithMessage("Password must be at least 8 characters long and contain at least one digit.");
		}
	}
}
