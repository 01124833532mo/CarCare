using AutoMapper;
using CarCare.Core.Application.Services.Contacts;
using CarCare.Core.Domain.Contracts.Persistence;
using CarCare.Core.Domain.Entities.Contacts;
using CarCare.Core.Domain.Entities.Identity;
using CarCare.Shared.ErrorModoule.Exeptions;
using CareCare.Core.Application.Abstraction.Models.Contacts;
using FakeItEasy;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace CarCare.Application.Testss.Services.Contacts
{
    public class ContactServiceTest
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ContactService _contactService;

        public ContactServiceTest()
        {
            _unitOfWork = A.Fake<IUnitOfWork>();
            _mapper = A.Fake<IMapper>();
            _userManager = A.Fake<UserManager<ApplicationUser>>(
                x => x.WithArgumentsForConstructor(() =>
                    new UserManager<ApplicationUser>(
                        A.Fake<IUserStore<ApplicationUser>>(),
                        null!, null!, null!, null!, null!, null!, null!, null!)));

            _contactService = new ContactService(_unitOfWork, _mapper, _userManager);
        }
        [Fact]
        public async Task CreateContactAsync_ShouldReturnCreatedContact_WhenContactIsValid()
        {
            // Arrange
            CreateContactDto contactDto = GenerateValidContactDto();
            ApplicationUser user = GenerateUserTest();
            Contact contact = GenerateTestContact(contactDto, user);

            A.CallTo(() => _mapper.Map<Contact>(contactDto)).Returns(contact);
            A.CallTo(() => _unitOfWork.GetRepository<Contact, int>().AddAsync(contact)).Returns(Task.CompletedTask);
            A.CallTo(() => _unitOfWork.CompleteAsync()).Returns(Task.FromResult(1));
            // Act
            var result = await _contactService.CreateContactAsync(contactDto);
            // Assert
            Assert.NotNull(result);
            Assert.Equal(contact.Message, result.Message);



        }
        [Fact]
        public async Task CreateContactAsync_RepoThrowsException_ThrowsBadRequest()
        {
            // Arrange
            var contactDto = GenerateValidContactDto();

            var user = GenerateUserTest();
            var contact = GenerateTestContact(contactDto, user);
            var fakeRepo = A.Fake<IGenericRepository<Contact, int>>();

            A.CallTo(() => _unitOfWork.GetRepository<Contact, int>())
                .Returns(fakeRepo);

            A.CallTo(() => _mapper.Map<Contact>(contactDto))
                .Returns(contact);

            A.CallTo(() => fakeRepo.AddAsync(contact))
                .ThrowsAsync(new Exception("Database error"));

            // Act & Assert
            await Assert.ThrowsAsync<BadRequestExeption>(() =>
                _contactService.CreateContactAsync(contactDto));
        }

        [Fact]
        public async Task CreateContactAsync_UserNotFound_ThrowsNotFoundException()
        {
            // Arrange
            var contactDto = GenerateValidContactDto();
            var contact = GenerateTestContact(contactDto, GenerateUserTest());

            A.CallTo(() => _mapper.Map<Contact>(contactDto)).Returns(contact);

            A.CallTo(() => _unitOfWork.GetRepository<Contact, int>().AddAsync(contact)).Returns(Task.CompletedTask);
            A.CallTo(() => _unitOfWork.CompleteAsync()).Returns(Task.FromResult(1));
            A.CallTo(() => _userManager.FindByIdAsync(contact.UserId))!.Returns(Task.FromResult<ApplicationUser>(null!));

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundExeption>(() =>
                _contactService.CreateContactAsync(contactDto));
        }
        [Fact]
        public async Task CreateContactAsync_UserThrowsException_ThrowsBadRequest()
        {
            // Arrange
            var contactDto = GenerateValidContactDto();
            var contact = GenerateTestContact(contactDto, GenerateUserTest());
            A.CallTo(() => _mapper.Map<Contact>(contactDto)).Returns(contact);
            A.CallTo(() => _unitOfWork.GetRepository<Contact, int>().AddAsync(contact)).Returns(Task.CompletedTask);
            A.CallTo(() => _unitOfWork.CompleteAsync()).Returns(Task.FromResult(1));
            A.CallTo(() => _userManager.FindByIdAsync(contact.UserId)).Throws(new Exception("User service error"));
            // Act & Assert
            await Assert.ThrowsAsync<BadRequestExeption>(() =>
                _contactService.CreateContactAsync(contactDto));
        }
        [Fact]
        public async Task CreateContactAsync_UserIdIsNull_ThrowsBadRequest()
        {
            // Arrange
            var contactDto = GenerateValidContactDto();
            var contact = GenerateTestContact(contactDto, GenerateUserTest());
            contact.UserId = null!; // Simulate null UserId
            A.CallTo(() => _mapper.Map<Contact>(contactDto)).Returns(contact);
            A.CallTo(() => _unitOfWork.GetRepository<Contact, int>().AddAsync(contact)).Returns(Task.CompletedTask);
            A.CallTo(() => _unitOfWork.CompleteAsync()).Returns(Task.FromResult(1));
            A.CallTo(() => _userManager.FindByIdAsync(contact.UserId!))!.Returns(Task.FromResult<ApplicationUser>(null!));
            // Act & Assert
            await Assert.ThrowsAsync<BadRequestExeption>(() =>
                _contactService.CreateContactAsync(contactDto));
        }
        [Fact]
        public async Task CreateContactAsync_UserFullNameIsNull_ThrowsBadRequest()
        {
            // Arrange
            var contactDto = GenerateValidContactDto();
            var user = GenerateUserTest();
            user.FullName = null!; // Simulate null FullName
            var contact = GenerateTestContact(contactDto, user);
            A.CallTo(() => _mapper.Map<Contact>(contactDto)).Returns(contact);
            A.CallTo(() => _unitOfWork.GetRepository<Contact, int>().AddAsync(contact)).Returns(Task.CompletedTask);
            A.CallTo(() => _unitOfWork.CompleteAsync()).Returns(Task.FromResult(1));
            A.CallTo(() => _userManager.FindByIdAsync(contact.UserId))!.Returns(Task.FromResult(user));
            // Act & Assert
            await Assert.ThrowsAsync<BadRequestExeption>(() =>
                _contactService.CreateContactAsync(contactDto));
        }

        [Fact]
        public async Task CreateContactAsync_UserFullNameIsEmpty_ThrowsBadRequest()
        {
            // Arrange
            var contactDto = GenerateValidContactDto();
            var user = GenerateUserTest();
            user.FullName = string.Empty; // Simulate empty FullName
            var contact = GenerateTestContact(contactDto, user);
            A.CallTo(() => _mapper.Map<Contact>(contactDto)).Returns(contact);
            A.CallTo(() => _unitOfWork.GetRepository<Contact, int>().AddAsync(contact)).Returns(Task.CompletedTask);
            A.CallTo(() => _unitOfWork.CompleteAsync()).Returns(Task.FromResult(1));
            A.CallTo(() => _userManager.FindByIdAsync(contact.UserId))!.Returns(Task.FromResult(user));
            // Act & Assert
            await Assert.ThrowsAsync<BadRequestExeption>(() =>
                _contactService.CreateContactAsync(contactDto));
        }
        [Fact]
        public async Task CreateContactAsync_UserFullNameIsWhitespace_ThrowsBadRequest()
        {
            // Arrange
            var contactDto = GenerateValidContactDto();
            var user = GenerateUserTest();
            user.FullName = "Test_name"; // Simulate whitespace FullName
            var contact = GenerateTestContact(contactDto, user);
            A.CallTo(() => _mapper.Map<Contact>(contactDto)).Returns(contact);
            A.CallTo(() => _unitOfWork.GetRepository<Contact, int>().AddAsync(contact)).Returns(Task.CompletedTask);
            A.CallTo(() => _unitOfWork.CompleteAsync()).Returns(Task.FromResult(1));
            A.CallTo(() => _userManager.FindByIdAsync(contact.UserId))!.Returns(Task.FromResult(user));
            // Act & Assert
            await Assert.ThrowsAsync<BadRequestExeption>(() =>
                _contactService.CreateContactAsync(contactDto));
        }

        [Fact]
        public async Task GetAllContactsAsync_WhenUserIdNotFound_ThrowsUnAuthorizedException()
        {
            // Arrange
            var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new[]
            {
        new Claim(ClaimTypes.PrimarySid, "nonexistent-user-id")
             }));

            A.CallTo(() => _userManager.FindByIdAsync(A<string>._))!
                .Returns(Task.FromResult<ApplicationUser>(null!));

            // Act & Assert
            await Assert.ThrowsAsync<UnAuthorizedExeption>(() =>
                _contactService.GetAllContactsAsync(claimsPrincipal));
        }
        [Fact]
        public async Task GetAllContactsAsync_WhenUserIdFound_ReturnsContacts()
        {
            // Arrange
            var userId = Guid.NewGuid().ToString();
            var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.PrimarySid, userId)
            }));
            var user = new ApplicationUser { Id = userId, FullName = "Test User" };
            A.CallTo(() => _userManager.FindByIdAsync(userId))!.Returns(Task.FromResult(user));
            List<Contact> contacts = CreateContactList(userId);

            // Act
            var result = await _contactService.GetAllContactsAsync(claimsPrincipal);
            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());



        }


        #region Helper Functions
        private CreateContactDto GenerateValidContactDto()
        {
            return new CreateContactDto
            {
                Message = "Test message",
                MessageFor = 1
            };
        }
        private ApplicationUser GenerateUserTest()
        {
            var user = new ApplicationUser
            {
                Id = new Guid(Guid.NewGuid().ToString()).ToString()
                ,
                FullName = "Test User" // Required property
            };
            return user;
        }

        private Contact GenerateTestContact(CreateContactDto contactDto, ApplicationUser user)
        {
            return new Contact
            {
                Id = 1,
                Message = contactDto.Message,
                MessageFor = Types.All,

                User = user,

                UserId = new Guid(Guid.NewGuid().ToString()).ToString()
            };
        }

        static List<Contact> CreateContactList(string userId)
        {
            var user = new ApplicationUser()
            {
                FullName = "Test User",
                Id =
                  new Guid(Guid.NewGuid().ToString()).ToString()
            };

            return new List<Contact>
            {
                new Contact { Id = 1, Message = "Contact 1",MessageFor=Types.All,User=user, UserId = userId },
                new Contact { Id = 2, Message = "Contact 2",MessageFor=Types.All,User=user,  UserId = userId }
            };
        }
        #endregion
    }
}
