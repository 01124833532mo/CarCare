using CareCare.Core.Application.Abstraction.Common;
using CareCare.Core.Application.Abstraction.Models.FeedBack;

namespace CareCare.Core.Application.Abstraction.Services.FeedBack
{
    public interface IFeedBackService
    {
        Task<ReturnFeedBackDto> CreateFeedBackAsync(CreateFeedBackDto feedBackDto);

        Task<ReturnFeedBackDto> UpdateFeedBackAsync(int id, UpdatedFeedBackDto feedBackDto);

        Task<IEnumerable<ReturnFeedBackDto>> GetAllFeedBackAsync(SpecParams specsParams);

        Task<ReturnFeedBackDto> GetFeedBackAsync(int id);

        Task<string> DeleteFeedBackAsync(int id);

    }
}
