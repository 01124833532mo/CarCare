using AutoMapper;
using CarCare.Core.Domain.Contracts.Persistence;
using CarCare.Core.Domain.Entities.FeedBacks;
using CarCare.Core.Domain.Specifications.SpecsHandlers;
using CarCare.Shared.ErrorModoule.Exeptions;
using CareCare.Core.Application.Abstraction.Common;
using CareCare.Core.Application.Abstraction.Models.FeedBack;
using CareCare.Core.Application.Abstraction.Services.FeedBack;
using System.Security.Claims;

namespace CarCare.Core.Application.Services.FeedBacks
{
	public class FeedBackService(IUnitOfWork _unitOfWork, IMapper _mapper) : IFeedBackService
	{
		public async Task<ReturnFeedBackDto> CreateFeedBackAsync(ClaimsPrincipal claims, CreateFeedBackDto feedBackDto)
		{

			var UserId = claims.FindFirst(ClaimTypes.PrimarySid)?.Value;

			if (UserId is null)
				throw new UnAuthorizedExeption("UnAuthorized , You Are Not Allowed");

			var _usersId = GetUsersIDThatAddFeedBack();

			if (_usersId is not null)
			{
				if (_usersId.Result.Contains(UserId))
					throw new BadRequestExeption("You Already Add FeedBack Please Update Your FeedBack");
			}

			var mappedFeedBack = _mapper.Map<FeedBack>(feedBackDto);

			var Added = _unitOfWork.GetRepository<FeedBack, int>().AddAsync(mappedFeedBack);
			if (Added is null)
				throw new BadRequestExeption("FeedBack Not Created!");

			var created = await _unitOfWork.CompleteAsync() > 0;
			if (!created)
				throw new BadRequestExeption("FeedBack Not Created!");

			var returnedFeedBack = _mapper.Map<ReturnFeedBackDto>(mappedFeedBack);

			return returnedFeedBack;

		}


		public async Task<decimal> GetAvgRating()
		{
			var feedBacks = await _unitOfWork.GetRepository<FeedBack, int>().GetAllAsync();

			var avgRating = feedBacks.Average(x => x.Rating);

			return avgRating;
		}


		public async Task<IEnumerable<ReturnFeedBackDto>> GetAllFeedBackAsync(SpecParams specsParams)
		{
			var spec = new FeedBackWithUserSpecifications(specsParams.Sort);

			var feedBack = await _unitOfWork.GetRepository<FeedBack, int>().GetAllWithSpecAsync(spec);

			var returnedData = _mapper.Map<IEnumerable<ReturnFeedBackDto>>(feedBack);

			return returnedData;

		}


		public async Task<ReturnFeedBackDto> GetFeedBackAsync(int id)
		{
			var spec = new FeedBackWithUserSpecifications(id);
			var feedBack = await _unitOfWork.GetRepository<FeedBack, int>().GetWithSpecAsync(spec);

			if (feedBack is null)
				throw new NotFoundExeption(nameof(feedBack), id);

			var returnedFeedBack = _mapper.Map<ReturnFeedBackDto>(feedBack);

			return returnedFeedBack;
		}


		public async Task<ReturnFeedBackDto> UpdateFeedBackAsync(ClaimsPrincipal claims, int id, UpdatedFeedBackDto feedBackDto)
		{
			var UserId = claims.FindFirst(ClaimTypes.PrimarySid)?.Value;

			if (UserId is null)
				throw new UnAuthorizedExeption("UnAuthorized , You Are Not Allowed");

			var feedBack = await _unitOfWork.GetRepository<FeedBack, int>().GetAsync(id);

			if (UserId != feedBack?.UserId)
				throw new UnAuthorizedExeption("UnAuthorized , You Are Not Allowed");

			if (feedBack is null)
				throw new NotFoundExeption(nameof(feedBack), id);


			feedBack.Id = id;
			if (feedBackDto.Comment is not null)
				feedBack.Comment = feedBackDto.Comment;
			feedBack.Rating = feedBackDto.Rating;


			var Updated = await _unitOfWork.CompleteAsync() > 0;

			if (!Updated)
				throw new BadRequestExeption("You are not Update Your FeedBack ==> (Rating)");



			var returnedFeedBack = _mapper.Map<ReturnFeedBackDto>(feedBack);


			return returnedFeedBack;

		}


		public async Task<string> DeleteFeedBackAsync(ClaimsPrincipal claims, int id)
		{
			var UserId = claims.FindFirst(ClaimTypes.PrimarySid)?.Value;

			if (UserId is null)
				throw new UnAuthorizedExeption("UnAuthorized , You Are Not Allowed");

			var feedBack = await _unitOfWork.GetRepository<FeedBack, int>().GetAsync(id);

			if (UserId != feedBack?.UserId)
				throw new UnAuthorizedExeption("UnAuthorized , You Are Not Allowed");

			if (feedBack is null)
				throw new NotFoundExeption(nameof(feedBack), id);

			_unitOfWork.GetRepository<FeedBack, int>().Delete(feedBack);

			var deleted = await _unitOfWork.CompleteAsync() > 0;

			if (deleted)
				return "FeedBack Deleted Successfully";
			else
				throw new BadRequestExeption("Deleting Failed");

		}


		private async Task<List<string>> GetUsersIDThatAddFeedBack()
		{
			var feedBacks = await _unitOfWork.GetRepository<FeedBack, int>().GetAllAsync();

			var returnedData = _mapper.Map<IEnumerable<ReturnFeedBackDto>>(feedBacks);

			var UsersId = feedBacks.Select(f => f.UserId).ToList();

			return UsersId;

		}
	}
}
