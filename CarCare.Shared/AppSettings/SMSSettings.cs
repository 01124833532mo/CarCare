using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarCare.Shared.AppSettings
{
	public class SMSSettings
	{
		public required string AccountSID { get; set; }
		public required string AuthToken { get; set; }
		public required string TwilioPhoneNumber { get; set; }
	}
}
