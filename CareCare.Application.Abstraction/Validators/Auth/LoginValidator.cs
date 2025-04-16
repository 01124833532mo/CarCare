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
                .NotEmpty()
                .WithMessage("PhoneNumber Must Not Empty , Plz Add a {PropertyName}")
                .Matches(RegexPatterns.PhoneNumber).WithMessage("Invalid Egyptian phone number.");

            RuleFor(x => x.Password)
                .NotEmpty()
                .WithMessage("Password Must Not Empty , Plz Add a {PropertyName}");


        }
    }
}
