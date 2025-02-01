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
    }
}
