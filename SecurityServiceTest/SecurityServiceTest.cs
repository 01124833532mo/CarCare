using AutoMapper;
using CarCare.Core.Application.Services.Auth;
using CarCare.Core.Domain.Contracts.Persistence;
using CarCare.Core.Domain.Entities.Identity;
using CarCare.Shared.ErrorModoule.Exeptions;
using CareCare.Core.Application.Abstraction.Models.Auth;
using CareCare.Core.Application.Abstraction.Models.Auth._Common;
using CareCare.Core.Application.Abstraction.Services;
using FakeItEasy;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace SecurityServiceTest
{
    public class SecurityServiceTest
    {
        private readonly UserManager<ApplicationUser> _fakeUserManager;
        private readonly SignInManager<ApplicationUser> _fakeSignInManager;
        private readonly IUnitOfWork _fakeUnitOfWork;
        private readonly AuthService _authService;
        private readonly JwtSettings _jwtSettings = new() { Key = "test-secret-key", JWTRefreshTokenExpire = 15, DurationInMinutes = 3, Audience = "test_Audence", Issuer = "Test_Issure" };

        public SecurityServiceTest()
        {
            _fakeUserManager = A.Fake<UserManager<ApplicationUser>>(
                x => x.WithArgumentsForConstructor(() =>
                    new UserManager<ApplicationUser>(
                        A.Fake<IUserStore<ApplicationUser>>(),
                        null!, null!, null!, null!, null!, null!, null!, null!)));

            _fakeSignInManager = A.Fake<SignInManager<ApplicationUser>>(
                x => x.WithArgumentsForConstructor(() =>
                    new SignInManager<ApplicationUser>(
                        _fakeUserManager,
                        A.Fake<IHttpContextAccessor>(),
                        A.Fake<IUserClaimsPrincipalFactory<ApplicationUser>>(),
                        null!, null!, null!, null!)));

            _fakeUnitOfWork = A.Fake<IUnitOfWork>();

            _authService = new AuthService(
                Options.Create(_jwtSettings),
                _fakeUserManager,
                _fakeSignInManager,
                A.Fake<RoleManager<IdentityRole>>(),
                A.Fake<ISMSServices>(),
                A.Fake<IMapper>(),
                A.Fake<IEmailServices>(),
                _fakeUnitOfWork);
        }




        [Fact]
        public async Task LoginAsync_UserNotConfirmed_ThrowsUnauthorizedException()
        {
            // Arrange
            var user = CreateTestUser(emailConfirmed: false);
            A.CallTo(() => _fakeUserManager.FindByEmailAsync(user.Email)).Returns(user);
            // Act & Assert
            await Assert.ThrowsAsync<UnAuthorizedExeption>(() => _authService.LoginAsync(new LoginDto { PhoneNumber = user.PhoneNumber!, Password = "password" }));
        }
        [Fact]
        public async Task LoginAsync_UserTechnicalType_ThrowsUnAuthorizedException()
        {
            // Arrange
            var user = CreateTestUser(type: Types.Technical);
            A.CallTo(() => _fakeUserManager.FindByEmailAsync(user.Email!)).Returns(user);
            // Act & Assert
            await Assert.ThrowsAsync<UnAuthorizedExeption>(() => _authService.LoginAsync(new LoginDto { PhoneNumber = user.PhoneNumber!, Password = "password" }));
        }
        [Fact]
        public async Task LoginAsync_ValidUser_ReturnsToken()
        {
            // Arrange
            var user = CreateTestUser();
            A.CallTo(() => _fakeUserManager.FindByEmailAsync(user.Email!)).Returns(user);
            A.CallTo(() => _fakeSignInManager.PasswordSignInAsync(user, "password", false, false))
                .Returns(Task.FromResult(SignInResult.Success));
            // Act
            var result = await _authService.LoginAsync(new LoginDto { PhoneNumber = user.PhoneNumber!, Password = "password" });
            // Assert
            Assert.NotNull(result);
            Assert.NotEmpty(result.Token);
        }
        [Fact]
        public async Task LoginAsync_InvalidPassword_ThrowsUnAuthorizedException()
        {
            // Arrange
            var user = CreateTestUser();
            A.CallTo(() => _fakeUserManager.FindByEmailAsync(user.Email!)).Returns(user);
            A.CallTo(() => _fakeSignInManager.PasswordSignInAsync(user, "wrongpassword", false, false))
                .Returns(Task.FromResult(SignInResult.Failed));
            // Act & Assert
            await Assert.ThrowsAsync<UnAuthorizedExeption>(() => _authService.LoginAsync(new LoginDto { PhoneNumber = user.PhoneNumber!, Password = "wrongpassword" }));
        }


        private ApplicationUser CreateTestUser(bool emailConfirmed = true, Types type = Types.User)
        {
            return new ApplicationUser
            {
                Id = "user-123",
                PhoneNumber = "1234567890",
                Email = "test@example.com",
                FullName = "Test User",
                EmailConfirmed = emailConfirmed,
                Type = type,
                ServiceId = type == Types.Technical ? 1 : null
            };
        }

    }
}