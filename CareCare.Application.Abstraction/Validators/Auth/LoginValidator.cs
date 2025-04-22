using CareCare.Core.Application.Abstraction.Models.Auth._Common;
using FluentValidation;

namespace CareCare.Core.Application.Abstraction.Validators.Auth
{
    public class LoginValidator : AbstractValidator<LoginDto>
    {
        public LoginValidator()
        {

            RuleFor(x => x.Password)
                .NotNull()
                .WithMessage("Password Must Not Null ,Plz Add a {PropertyName}")
                .NotEmpty()
                .WithMessage("Password Must Not Empty , Plz Add a {PropertyName}");


        }
    }
}
