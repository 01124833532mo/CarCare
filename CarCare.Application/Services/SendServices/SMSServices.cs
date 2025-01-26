using CarCare.Shared.AppSettings;
using CareCare.Core.Application.Abstraction.Models.Auth.ForgetPassword;
using CareCare.Core.Application.Abstraction.Services;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace CarCare.Core.Application.Services.SMS
{
	public class SMSServices(IOptions<SMSSettings> smsSettings) : ISMSServices
	{
		private readonly SMSSettings _smsSettings = smsSettings.Value;

		public async Task<MessageResource> SendSMS(SMSDto sms)
		{
			//1. Start Connection With Twilio
			TwilioClient.Init(_smsSettings.AccountSID, _smsSettings.AuthToken);
			//2. Create Sms
			var Result = await MessageResource.CreateAsync(
				body: sms.Body,
				from: new PhoneNumber(_smsSettings.TwilioPhoneNumber),
				to: $"+2{sms.PhoneNumber}"
				);
			//3. Returnning The Message
			return Result;
		}
	}
}
