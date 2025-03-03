using AutoMapper;
using CarCare.Core.Domain.Contracts.Persistence;
using CarCare.Core.Domain.Entities.Identity;
using CarCare.Core.Domain.Entities.Vehicles;
using CarCare.Core.Domain.Specifications.SpecsHandlers.Vehicles;
using CarCare.Shared.ErrorModoule.Exeptions;
using CareCare.Core.Application.Abstraction.Common;
using CareCare.Core.Application.Abstraction.Models.Vehicles;
using CareCare.Core.Application.Abstraction.Services.Vehicles;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace CarCare.Core.Application.Services.Vehicles
{
    public class VehicleService(IUnitOfWork _unitOfWork, IMapper _mapper, UserManager<ApplicationUser> userManager) : IVehicleService
    {
        public async Task<Pagination<VehicleToReturn>> GetAllVehicles(SpecParams specParams)
        {
            return await SetSpecificationsForGetAllVehicles(_unitOfWork, _mapper, specParams, specParams.UserId);


        }

        public async Task<Pagination<VehicleToReturn>> GetAllVehicleForUser(ClaimsPrincipal claimsPrincipal, SpecParams specParams)
        {
            var userid = claimsPrincipal.FindFirstValue(ClaimTypes.PrimarySid);
            if (userid is null) throw new NotFoundExeption("No User Found For This Id", userid!);

            return await SetSpecificationsForGetAllVehicles(_unitOfWork, _mapper, specParams, userid);

        }

        private static async Task<Pagination<VehicleToReturn>> SetSpecificationsForGetAllVehicles(IUnitOfWork _unitOfWork, IMapper _mapper, SpecParams specParams, string? userid)
        {
            var spec = new VehicleWithUserSpecifications(specParams.Sort, userid, specParams.PageSize, specParams.PageIndex, specParams.Search);



            var Vehicles = await _unitOfWork.GetRepository<Vehicle, int>().GetAllWithSpecAsync(spec);

            var data = _mapper.Map<IEnumerable<VehicleToReturn>>(Vehicles);


            var countSpec = new VehicleWithFilterationForCountSpecifications(userid, specParams.Search);
            var count = await _unitOfWork.GetRepository<Vehicle, int>().GetCountAsync(countSpec);

            return new Pagination<VehicleToReturn>(specParams.PageIndex, specParams.PageSize, count) { Data = data };
        }

        public async Task<VehicleToReturn> GetVehicle(int id)
        {
            var spec = new VehicleWithUserSpecifications(id);

            var Vehicle = await _unitOfWork.GetRepository<Vehicle, int>().GetWithSpecAsync(spec);
            if (Vehicle is null)
                throw new NotFoundExeption(nameof(Vehicle), id);

            var returnedVehicle = _mapper.Map<VehicleToReturn>(Vehicle);

            return returnedVehicle;

        }
        public async Task<VehicleToReturn> CreateVehicle(CreateVehicleDto createVehicleDto)
        {

            var checkPlateNumber = _unitOfWork.VehicleRepository.CheckPlateNumberExist(createVehicleDto.PlateNumber);

            if (checkPlateNumber is true) throw new BadRequestExeption("The PlateNumber Already Exsists Please Enter Anthor One");

            var checkVinNumber = _unitOfWork.VehicleRepository.CheckVINNumberExist(createVehicleDto.VIN_Number);

            if (checkVinNumber is true) throw new BadRequestExeption("The Vin Number Already Exsists Please Enter Anthor One");

            var mappedresult = _mapper.Map<Vehicle>(createVehicleDto);
            mappedresult.NormatizedVIN_Number = createVehicleDto.VIN_Number;


            var AddResult = _unitOfWork.GetRepository<Vehicle, int>().AddAsync(mappedresult);
            if (AddResult is null) throw new BadRequestExeption("Operation Not Succeded");

            var Created = await _unitOfWork.CompleteAsync() > 0;

            if (!Created) throw new BadRequestExeption("an error has occured during creating the Vehicle");


            var resultToReturn = _mapper.Map<VehicleToReturn>(mappedresult);

            var FullNameUser = await userManager.FindByIdAsync(resultToReturn.UserId);

            if (FullNameUser == null)
            {
                throw new BadRequestExeption("User not found");
            }

            resultToReturn.FullName = FullNameUser.FullName;
            return resultToReturn;

        }

        public async Task<string> DeleteVehicle(int id)
        {
            var repo = _unitOfWork.GetRepository<Vehicle, int>();
            var vehicle = await repo.GetAsync(id);

            if (vehicle is null) throw new NotFoundExeption("Not Vehicle With This Id:", id);

            repo.Delete(vehicle);

            var result = await _unitOfWork.CompleteAsync() > 0;

            if (result is true) return "Deleted Successfully";

            else
                throw new BadRequestExeption("Operation Faild");


        }


    }
}
