using CarCare.Shared.Models._Common;
using CareCare.Core.Application.Abstraction.Models.Auth.UpdatingUsersDtos;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CareCare.Core.Application.Abstraction.Validators.Auth
{
	public class UpdateAppUserBySelfValidator : AbstractValidator<UpdateTechDto>
	{
		public UpdateAppUserBySelfValidator()
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


			RuleFor(x => x.NationalId)
			  .Length(14)
			  .WithMessage("Egyptian national ID must be exactly 14 digits")
			  .Matches(RegexPatterns.NationalId)
			  .WithMessage("National ID must start with 2 (for 1900s) or 3 (for 2000s) followed by 13 digits")
			  .Must(BeAValidNationalIdDate)
			  .WithMessage("Invalid birth date in national ID")
			  .When(x => !string.IsNullOrWhiteSpace(x.NationalId));


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
