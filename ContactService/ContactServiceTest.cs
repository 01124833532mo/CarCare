using AutoMapper;
using CarCare.Core.Domain.Contracts.Persistence;
using CarCare.Core.Domain.Entities.Identity;
using FakeItEasy;
using Microsoft.AspNetCore.Identity;

namespace ContactService
{
    internal class ContactServiceTest
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ContactService _contactService;

        public ContactServiceTests()
        {
            _unitOfWork = A.Fake<IUnitOfWork>();
            _mapper = A.Fake<IMapper>();
            _userManager = A.Fake<UserManager<ApplicationUser>>(
                x => x.WithArgumentsForConstructor(() =>
                    new UserManager<ApplicationUser>(
                        A.Fake<IUserStore<ApplicationUser>>(),
                        null, null, null, null, null, null, null, null)));

            _contactService = new ContactService(_unitOfWork, _mapper, _userManager);
        }

    }
}
