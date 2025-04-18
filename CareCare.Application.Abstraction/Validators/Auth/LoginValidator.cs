using CarCare.Shared.Models._Common;
using CareCare.Core.Application.Abstraction.Models.Auth._Common;
using FluentValidation;

namespace CareCare.Core.Application.Abstraction.Validators.Auth
{
	public class LoginValidator : AbstractValidator<LoginDto>
	{
		public LoginValidator()
		{
			RuleFor(x => x.PhoneNumber)
				.NotNull()
				.WithMessage("Phone Number Must Not Null ,Plz Add a {PropertyName}")
				.NotEmpty()
				.WithMessage("Phone Number Must Not Empty , Plz Add a {PropertyName}")
				.Matches(RegexPatterns.PhoneNumber).WithMessage("Invalid Egyptian phone number.");

			RuleFor(x => x.Password)
				.NotNull()
				.WithMessage("Password Must Not Null ,Plz Add a {PropertyName}")
				.NotEmpty()
				.WithMessage("Password Must Not Empty , Plz Add a {PropertyName}");


		}
	}
}
