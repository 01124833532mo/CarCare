using AutoMapper;
using CarCare.Core.Domain.Contracts.Persistence;
using CarCare.Core.Domain.Entities.FeedBacks;
using CarCare.Core.Domain.Entities.Identity;
using CarCare.Core.Domain.Entities.Vehicles;
using CarCare.Shared.ErrorModoule.Exeptions;
using CareCare.Core.Application.Abstraction.Models.FeedBack;
using CareCare.Core.Application.Abstraction.Services.FeedBack;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace CarCare.Core.Application.Services.FeedBacks
{
	public class FeedBackService(IUnitOfWork _unitOfWork, IMapper _mapper) : IFeedBackService
	{
		public async Task<ReturnFeedBackDto> CreateFeedBackAsync(CreateFeedBackDto feedBackDto)
		{
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

		public async Task<ReturnFeedBackDto> GetFeedBackAsync(int id)
		{
			var feedBack = await _unitOfWork.GetRepository<FeedBack, int>().GetAsync(id);

			if (feedBack is null)
				throw new NotFoundExeption(nameof(feedBack), id);

			var returnedFeedBack = _mapper.Map<ReturnFeedBackDto>(feedBack);

			return returnedFeedBack;
		}

		public async Task<ReturnFeedBackDto> UpdateFeedBackAsync(UpdatedFeedBackDto feedBackDto)
		{

			var feedBack = await _unitOfWork.GetRepository<FeedBack, int>().GetAsync(feedBackDto.Id);



			if (feedBack is null)
				throw new NotFoundExeption(nameof(feedBack), feedBackDto.Id);


			feedBack.Id = feedBackDto.Id;
			feedBack.Comment = feedBackDto.Comment;
			feedBack.Rating = feedBackDto.Rating;

			var Updated = await _unitOfWork.CompleteAsync() > 0;

			if (!Updated)
				throw new BadRequestExeption("Opertaion isn't successeded");


			var returnedFeedBack = _mapper.Map<ReturnFeedBackDto>(feedBack);


			return returnedFeedBack;

		}

		public async Task<string> DeleteFeedBackAsync(int id)
		{
			var feedBack = await _unitOfWork.GetRepository<FeedBack, int>().GetAsync(id);

			if (feedBack is null)
				throw new NotFoundExeption(nameof(feedBack), id);

			_unitOfWork.GetRepository<FeedBack, int>().Delete(feedBack);

			var deleted = await _unitOfWork.CompleteAsync() > 0;

			if (deleted)
				return "FeedBack Deleted Successfully";
			else
				throw new BadRequestExeption("Deleting Failed");

		}

	}
}
