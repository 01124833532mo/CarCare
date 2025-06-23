using AutoMapper;
using CarCare.Core.Application.Services.Vehicles;
using CarCare.Core.Domain.Contracts.Persistence;
using CarCare.Core.Domain.Entities.Identity;
using CarCare.Core.Domain.Entities.Vehicles;
using CarCare.Shared.ErrorModoule.Exeptions;
using CareCare.Core.Application.Abstraction.Models.Vehicles;
using FakeItEasy;
using Microsoft.AspNetCore.Identity;

namespace VehicleServiceTest
{
    public class VehicleServiceTest
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly VehicleService _vehicleService;

        public VehicleServiceTest()
        {
            _unitOfWork = A.Fake<IUnitOfWork>();
            _mapper = A.Fake<IMapper>();
            _userManager = A.Fake<UserManager<ApplicationUser>>(
                x => x.WithArgumentsForConstructor(() =>
                    new UserManager<ApplicationUser>(
                        A.Fake<IUserStore<ApplicationUser>>(),
                        null!, null!, null!, null!, null!, null!, null!, null!)));

            _vehicleService = new VehicleService(_unitOfWork, _mapper, _userManager);
        }

        [Fact]
        public async Task CreateVehicle_ValidInput_ReturnsVehicleToReturn()
        {
            // Arrange
            var createDto = GenerateValidCreateVehicleDto();
            var vehicle = GenerateVehicle(createDto);
            var user = GenerateUser();
            var expectedResult = GenerateVehicleToReturn(vehicle, user);

            A.CallTo(() => _unitOfWork.VehicleRepository.CheckPlateNumberExist(createDto.PlateNumber))
                .Returns(false);
            A.CallTo(() => _unitOfWork.VehicleRepository.CheckVINNumberExist(createDto.VIN_Number))
                .Returns(false);
            A.CallTo(() => _mapper.Map<Vehicle>(createDto)).Returns(vehicle);
            A.CallTo(() => _unitOfWork.GetRepository<Vehicle, int>().AddAsync(vehicle))
                .Returns(Task.CompletedTask);
            A.CallTo(() => _unitOfWork.CompleteAsync()).Returns(1);
            A.CallTo(() => _mapper.Map<VehicleToReturn>(vehicle)).Returns(expectedResult);
            A.CallTo(() => _userManager.FindByIdAsync(vehicle.UserId))!
                .Returns(Task.FromResult(user));

            // Act
            var result = await _vehicleService.CreateVehicle(createDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedResult.PlateNumber, result.PlateNumber);
            Assert.Equal(expectedResult.VIN_Number, result.VIN_Number);
            Assert.Equal(user.FullName, result.FullName);
        }

        [Fact]
        public async Task CreateVehicle_DuplicatePlateNumber_ThrowsBadRequest()
        {
            // Arrange
            var createDto = GenerateValidCreateVehicleDto();

            A.CallTo(() => _unitOfWork.VehicleRepository.CheckPlateNumberExist(createDto.PlateNumber))
                .Returns(true);

            // Act & Assert
            await Assert.ThrowsAsync<BadRequestExeption>(() =>
                _vehicleService.CreateVehicle(createDto));
        }
        [Fact]
        public async Task CreateVehicle_DuplicateVINNumber_ThrowsBadRequest()
        {
            // Arrange
            var createDto = GenerateValidCreateVehicleDto();

            A.CallTo(() => _unitOfWork.VehicleRepository.CheckPlateNumberExist(createDto.PlateNumber))
                .Returns(false);
            A.CallTo(() => _unitOfWork.VehicleRepository.CheckVINNumberExist(createDto.VIN_Number))
                .Returns(true);

            // Act & Assert
            await Assert.ThrowsAsync<BadRequestExeption>(() =>
                _vehicleService.CreateVehicle(createDto));
        }
        [Fact]
        public async Task CreateVehicle_RepositoryThrowsException_ThrowsBadRequest()
        {
            // Arrange
            var createDto = GenerateValidCreateVehicleDto();
            var vehicle = GenerateVehicle(createDto);

            A.CallTo(() => _unitOfWork.VehicleRepository.CheckPlateNumberExist(createDto.PlateNumber))
                .Returns(false);
            A.CallTo(() => _unitOfWork.VehicleRepository.CheckVINNumberExist(createDto.VIN_Number))
                .Returns(false);
            A.CallTo(() => _mapper.Map<Vehicle>(createDto)).Returns(vehicle);
            A.CallTo(() => _unitOfWork.GetRepository<Vehicle, int>().AddAsync(vehicle))
                .ThrowsAsync(new Exception("Database error"));

            // Act & Assert
            await Assert.ThrowsAsync<BadRequestExeption>(() =>
                _vehicleService.CreateVehicle(createDto));
        }

        [Fact]
        public async Task CreateVehicle_UnitOfWorkFailsToSave_ThrowsBadRequest()
        {
            // Arrange
            var createDto = GenerateValidCreateVehicleDto();
            var vehicle = GenerateVehicle(createDto);

            A.CallTo(() => _unitOfWork.VehicleRepository.CheckPlateNumberExist(createDto.PlateNumber))
                .Returns(false);
            A.CallTo(() => _unitOfWork.VehicleRepository.CheckVINNumberExist(createDto.VIN_Number))
                .Returns(false);
            A.CallTo(() => _mapper.Map<Vehicle>(createDto)).Returns(vehicle);
            A.CallTo(() => _unitOfWork.GetRepository<Vehicle, int>().AddAsync(vehicle))
                .Returns(Task.CompletedTask);
            A.CallTo(() => _unitOfWork.CompleteAsync()).Returns(0);

            // Act & Assert
            await Assert.ThrowsAsync<BadRequestExeption>(() =>
                _vehicleService.CreateVehicle(createDto));
        }
        [Fact]
        public async Task CreateVehicle_UserNotFound_ThrowsBadRequest()
        {
            // Arrange
            var createDto = GenerateValidCreateVehicleDto();
            var vehicle = GenerateVehicle(createDto);
            var expectedResult = GenerateVehicleToReturn(vehicle, null);

            A.CallTo(() => _unitOfWork.VehicleRepository.CheckPlateNumberExist(createDto.PlateNumber))
                .Returns(false);
            A.CallTo(() => _unitOfWork.VehicleRepository.CheckVINNumberExist(createDto.VIN_Number))
                .Returns(false);
            A.CallTo(() => _mapper.Map<Vehicle>(createDto)).Returns(vehicle);
            A.CallTo(() => _unitOfWork.GetRepository<Vehicle, int>().AddAsync(vehicle))
                .Returns(Task.CompletedTask);
            A.CallTo(() => _unitOfWork.CompleteAsync()).Returns(1);
            A.CallTo(() => _mapper.Map<VehicleToReturn>(vehicle)).Returns(expectedResult);
            A.CallTo(() => _userManager.FindByIdAsync(vehicle.UserId))!
                .Returns(Task.FromResult<ApplicationUser>(null!));

            // Act & Assert
            await Assert.ThrowsAsync<BadRequestExeption>(() =>
                _vehicleService.CreateVehicle(createDto));
        }
        [Fact]
        public async Task CreateVehicle_NullInput_ThrowsBadRequest()
        {
            // Act & Assert
            await Assert.ThrowsAsync<BadRequestExeption>(() =>
                _vehicleService.CreateVehicle(null!));
        }
        private CreateVehicleDto GenerateValidCreateVehicleDto()
        {
            return new CreateVehicleDto
            {
                PlateNumber = "ABC123",
                VIN_Number = "1HGBH41JXMN109186",
                Color = "Red",
                Model = "Toyota Camry",
                Year = 2020,

                // Add other required properties
            };
        }

        private Vehicle GenerateVehicle(CreateVehicleDto dto)
        {
            return new Vehicle
            {
                PlateNumber = dto.PlateNumber,
                VIN_Number = dto.VIN_Number,
                NormatizedVIN_Number = dto.VIN_Number,
                Color = dto.Color,
                Model = dto.Model,
                Year = dto.Year,
                // Add other properties
            };
        }

        private ApplicationUser GenerateUser()
        {
            return new ApplicationUser
            {
                Id = "user123",
                FullName = "John Doe",
                // Add other properties
            };
        }

        private VehicleToReturn GenerateVehicleToReturn(Vehicle vehicle, ApplicationUser user)
        {
            return new VehicleToReturn
            {
                PlateNumber = vehicle.PlateNumber,
                VIN_Number = vehicle.VIN_Number,
                UserId = vehicle.UserId,
                FullName = user?.FullName ?? string.Empty,
                Color = vehicle.Color,
                Model = vehicle.Model,
                Year = vehicle.Year,
                Id = vehicle.Id,

                // Add other properties
            };
        }
    }
}