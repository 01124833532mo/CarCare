using CareCare.Core.Application.Abstraction.Common;
using CareCare.Core.Application.Abstraction.Models.FeedBack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace CareCare.Core.Application.Abstraction.Services.FeedBack
{
	public interface IFeedBackService
	{
		Task<ReturnFeedBackDto> CreateFeedBackAsync(ClaimsPrincipal claims, CreateFeedBackDto feedBackDto);

		Task<decimal> GetAvgRating();

		Task<ReturnFeedBackDto> UpdateFeedBackAsync(ClaimsPrincipal claims, int id, UpdatedFeedBackDto feedBackDto);

		Task<IEnumerable<ReturnFeedBackDto>> GetAllFeedBackAsync(SpecParams specsParams);

		Task<ReturnFeedBackDto> GetFeedBackAsync(int id);

		Task<string> DeleteFeedBackAsync(ClaimsPrincipal claims, int id);

	}
}
