using CarCare.Shared.Models._Common;
using CareCare.Core.Application.Abstraction.Models.Auth.RegisterDtos;
using FluentValidation;

namespace CareCare.Core.Application.Abstraction.Validators.Security
{
    public class RegisterForUserValidator : AbstractValidator<UserRegisterDto>
    {
        public RegisterForUserValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .WithMessage("\"Email Must Not Empty , Plz Add a {PropertyName}\"")
                .EmailAddress().WithMessage("Must Be Email Address");

            RuleFor(x => x.FullName).NotEmpty().WithMessage("FullName Must Not Empty , Plz Add a {PropertyName}");

            RuleFor(x => x.PhoneNumber)
                .NotEmpty()
                .WithMessage("PhoneNumber Must Not Empty , Plz Add a {PropertyName}")
                .Matches(RegexPatterns.PhoneNumber).WithMessage("Invalid Egyptian phone number.");


            RuleFor(x => x.Password)
                .NotEmpty()
                .WithMessage("\"Password Must Not Empty ,Plz Add a {PropertyName}\"")
                .Matches(RegexPatterns.Password).WithMessage("Password must be at least 8 characters long and contain at least one digit.");

            RuleFor(x => x.Type)
              .Must(type => (int)type == 1)
                 .WithMessage("Type must be  (User).");

        }
    }
}
