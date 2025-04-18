using CareCare.Core.Application.Abstraction.Models.Auth.ForgetPasswordByEmailDtos;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CareCare.Core.Application.Abstraction.Validators.Auth
{
	public class ResetPasswordValidator : AbstractValidator<ResetPasswordByEmailDto>
	{
		public ResetPasswordValidator()
		{
			RuleFor(x => x.NewPassword)
				.NotNull()
				.WithMessage(errorMessage: "New Password is required.")
				.NotEmpty()
				.WithMessage("New Password is required.")
				.MinimumLength(6)
				.WithMessage("Password must be at least 6 characters long.");

		}
	}
}
