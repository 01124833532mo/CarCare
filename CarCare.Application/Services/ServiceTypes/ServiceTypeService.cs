using AutoMapper;
using CarCare.Core.Domain.Contracts.Persistence;
using CarCare.Core.Domain.Entities.ServiceTypes;
using CarCare.Shared.ErrorModoule.Exeptions;
using CareCare.Core.Application.Abstraction.Models.ServiceTypes;
using CareCare.Core.Application.Abstraction.Services.ServiceTypes;

namespace CarCare.Core.Application.Services.ServiceTypes
{
    public class ServiceTypeService(IUnitOfWork _unitOfWork, IMapper _mapper) : IServiceTypeService
    {


        public async Task<IEnumerable<ServiceTypeToReturn>> GetAllServicesTypes()
        {

            var ServicesTypes = await _unitOfWork.GetRepository<ServiceType, int>().GetAllAsync();

            var mappedservices = _mapper.Map<IEnumerable<ServiceTypeToReturn>>(ServicesTypes);

            return mappedservices;

        }

        public async Task<ServiceTypeToReturn> GetServiceType(int id)
        {
            var servicetype = await _unitOfWork.GetRepository<ServiceType, int>().GetAsync(id);

            if (servicetype is null)
                throw new NotFoundExeption("Not Exsist For This Service", nameof(id));

            var mappedserviceType = _mapper.Map<ServiceTypeToReturn>(servicetype);

            return mappedserviceType;
        }

        public async Task<string> DeleteServiceType(int id)
        {
            var repo = _unitOfWork.GetRepository<ServiceType, int>();
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
