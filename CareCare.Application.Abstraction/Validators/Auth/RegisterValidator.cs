using CarCare.Shared.Models._Common;
using CareCare.Core.Application.Abstraction.Models.Auth.RegisterDtos;
using FluentValidation;
using System.Text.RegularExpressions;

namespace CareCare.Core.Application.Abstraction.Validators.Auth
{
    public class RegisterValidator : AbstractValidator<RegisterDto>
    {
        public RegisterValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .WithMessage("\"Email Must Not Empty , Plz Add a {PropertyName}\"")
                .EmailAddress().WithMessage("Must Be Email Address")
                .Matches(RegexPatterns.Email,
         RegexOptions.IgnoreCase).WithMessage("Invalid Email Address,Only Gmail/Google or Egyptian university emails (@____.edu.eg) are allowed");


            RuleFor(x => x.FullName).NotEmpty().WithMessage("FullName Must Not Empty , Plz Add a {PropertyName}");

            RuleFor(x => x.PhoneNumber)
                .NotEmpty()
                .WithMessage("PhoneNumber Must Not Empty , Plz Add a {PropertyName}")
                .Matches(RegexPatterns.PhoneNumber).WithMessage("Invalid Egyptian phone number.");


            RuleFor(x => x.Password)
                .NotEmpty()
                .WithMessage("\"Password Must Not Empty ,Plz Add a {PropertyName}\"")
                .Matches(RegexPatterns.Password).WithMessage("Password must be at least 8 characters long and contain at least one digit.");


            RuleFor(x => x.NationalId)
      .NotEmpty()
      .When(x => x.Type == 0)
      .WithMessage("NationalId Must Not Be Empty. Please enter a {PropertyName}")
      .Length(14)
      .When(x => x.Type == 0)
      .WithMessage("Egyptian national ID must be exactly 14 digits")
      .Matches(RegexPatterns.NationalId)
      .When(x => x.Type == 0)
      .WithMessage("National ID must start with 2 (for 1900s) or 3 (for 2000s) followed by 13 digits")
      .Must(BeAValidNationalIdDate)
      .When(x => x.Type == 0)
      .WithMessage("Invalid birth date in national ID");

            RuleFor(x => x.ServiceId)
                .NotEmpty()
                .When(x => x.Type == 0)
                .WithMessage("Service Id Must Be Assigned. Please enter {PropertyName}");





            RuleFor(x => x.Type)
                .Must(type => (int)type == 0 || (int)type == 1)
                     .WithMessage("Type must be  (Technical or User).");

        }

        private bool BeAValidNationalIdDate(string nationalId)
        {
            if (string.IsNullOrWhiteSpace(nationalId) || nationalId.Length != 14)
                return false;

            try
            {
                int century = nationalId[0] == '2' ? 1900 : 2000;
                int year = century + int.Parse(nationalId.Substring(1, 2));
                int month = int.Parse(nationalId.Substring(3, 2));
                int day = int.Parse(nationalId.Substring(5, 2));

                var birthDate = new DateTime(year, month, day);
                return birthDate <= DateTime.Now.Date && birthDate >= new DateTime(century, 1, 1);
            }
            catch
            {
                return false;
            }
        }
    }
}
