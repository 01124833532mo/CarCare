using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CareCare.Core.Application.Abstraction.Models.FeedBack
{
	public class ReturnFeedBackDto
	{
		public DateTime Date { get; set; }

		public string? Comment { get; set; }

		public decimal Rating { get; set; }


		public required string UserId { get; set; }
	}
}
