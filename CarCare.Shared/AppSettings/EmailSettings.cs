﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarCare.Shared.AppSettings
{
	public class EmailSettings
	{
		public required string Email { get; set; }
		public required string DisplayName { get; set; }
		public required string Password { get; set; }
		public required string Host { get; set; }
		public int Port { get; set; }
	}
}
