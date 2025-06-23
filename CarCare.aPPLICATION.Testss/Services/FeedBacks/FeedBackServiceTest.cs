using AutoMapper;
using CarCare.Core.Application.Services.FeedBacks;
using CarCare.Core.Domain.Contracts.Persistence;
using CarCare.Core.Domain.Entities.FeedBacks;
using CarCare.Core.Domain.Entities.Identity;
using CareCare.Core.Application.Abstraction.Models.FeedBack;
using FakeItEasy;
using System.Security.Claims;

namespace FeedBackServiceTest
{
    public class FeedBackServiceTest
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly FeedBackService _feedBackService;

        public FeedBackServiceTest()
        {
            _unitOfWork = A.Fake<IUnitOfWork>();
            _mapper = A.Fake<IMapper>();
            _feedBackService = new FeedBackService(_unitOfWork, _mapper);
        }
        [Fact]
        public async Task CreateFeedBackAsync_ValidInput_ReturnsFeedBackDto()
        {
            // Arrange
            var claims = CreateClaimsPrincipal("user123");
            var feedBackDto = new CreateFeedBackDto { Comment = "TEST Comment", Rating = 3.2m };
            ApplicationUser user = GenerateApplicationUser();
            var feedBack = new FeedBack { User = user, UserId = "user123", };
            var returnDto = new ReturnFeedBackDto { UserId = "user123", UserName = "testuser" };

            A.CallTo(() => _mapper.Map<FeedBack>(feedBackDto)).Returns(feedBack);
            A.CallTo(() => _unitOfWork.GetRepository<FeedBack, int>().AddAsync(feedBack)).Returns(Task.CompletedTask);
            A.CallTo(() => _unitOfWork.CompleteAsync()).Returns(Task.FromResult(1));
            A.CallTo(() => _mapper.Map<ReturnFeedBackDto>(feedBack)).Returns(returnDto);

            // Act
            var result = await _feedBackService.CreateFeedBackAsync(claims, feedBackDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(returnDto, result);


        }
        [Fact]
        public async Task CreateFeedBackAsync_InvalidInput_ThrowsException()
        {
            // Arrange
            var claims = CreateClaimsPrincipal("user123");
            var feedBackDto = new CreateFeedBackDto { Comment = "TEST Comment", Rating = 3.2m };
            var user = GenerateApplicationUser();
            var feedBack = new FeedBack { UserId = "user123", User = user };
            A.CallTo(() => _mapper.Map<FeedBack>(feedBackDto)).Returns(feedBack);
            A.CallTo(() => _unitOfWork.GetRepository<FeedBack, int>().AddAsync(feedBack)).Throws(new Exception("Database error"));
            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _feedBackService.CreateFeedBackAsync(claims, feedBackDto));
        }



        private ClaimsPrincipal CreateClaimsPrincipal(string userId)
        {
            return new ClaimsPrincipal(new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.PrimarySid, userId)
            }));
        }
        static ApplicationUser GenerateApplicationUser()
        {
            var user = new ApplicationUser
            {
                Id = "user123",
                UserName = "testuser",
                FullName = "resttttt"
            };
            return user;
        }



    }
}