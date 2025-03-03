using CareCare.Core.Application.Abstraction.Models.Auth.ForgetPassword;
using Twilio.Rest.Api.V2010.Account;
namespace CareCare.Core.Application.Abstraction.Services
{
	public interface ISMSServices
	{
		Task<MessageResource> SendSMS(SMSDto sms);
	}
}
