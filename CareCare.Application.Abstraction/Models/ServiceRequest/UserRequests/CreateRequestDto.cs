using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CareCare.Core.Application.Abstraction.Models.ServiceRequest.UserRequests
{
	public class CreateRequestDto
	{

		public required string TechId { get; set; }
		public int ServiceTypeId { get; set; }


		public string? BettaryType { get; set; }

		public int? LitersOfFuel { get; set; }
		public string? TypeOfFuel { get; set; }

		public int? LitersOfOil { get; set; }
		public string? TypeOfOil { get; set; }

		public int? TireCount { get; set; }
		public string? TireSize { get; set; }

		public string? TypeOfWinch { get; set; }

		public double? UserLatitude { get; set; }
		public double? UserLongitude { get; set; }

		public decimal ServicePrice { get; set; }
	}
}
