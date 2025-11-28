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
	internal class ChangePasswordValidator : AbstractValidator<ChangePasswordDto>
	{
		public ChangePasswordValidator()
		{
			RuleFor(x => x.CurrentPassword)
				.NotNull()
				.WithMessage("Current Password Must Not Null ,Plz Add a {PropertyName}")
				.NotEmpty()
				.WithMessage("Current Password Must Not Empty ,Plz Add a {PropertyName}");

			RuleFor(x => x.NewPassword)
				.NotNull()
				.WithMessage("New Password Must Not Null ,Plz Add a {PropertyName}")
				.NotEmpty()
				.WithMessage("Password Must Not Empty ,Plz Add a {PropertyName}")
				.Matches(RegexPatterns.Password)
				.WithMessage("Password must be at least 8 characters long and contain at least one digit.");
		}
	}
}
