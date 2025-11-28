using CareCare.Core.Application.Abstraction.Models.Contacts;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CareCare.Core.Application.Abstraction.Validators.Contact
{
	public class CreateContactValidator : AbstractValidator<CreateContactDto>
	{
		public CreateContactValidator()
		{

			RuleFor(x => x.Message)
				.NotNull()
				.WithMessage("Message Must Not Null ,Plz Add a {PropertyName}")
				.NotEmpty()
				.WithMessage("Message Must Not Empty ,Plz Add a {PropertyName}");

			RuleFor(x => x.MessageFor)
				.NotNull()
				.WithMessage("MessageFor Must Not Null ,Plz Add a {PropertyName}")
				.InclusiveBetween(0, 3)
				.WithMessage("MessageFor Must be between 0 to 3");
		}
	}
}
