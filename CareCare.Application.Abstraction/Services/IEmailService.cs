using CareCare.Core.Application.Abstraction.Models.Auth.ForgetPasswordByEmailDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CareCare.Core.Application.Abstraction.Services
{
	public interface IEmailServices
	{
		Task SendEmail(EmailDto emailDto);
	}
}
