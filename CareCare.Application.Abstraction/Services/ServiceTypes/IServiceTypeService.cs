using CareCare.Core.Application.Abstraction.Models.ServiceTypes;

namespace CareCare.Core.Application.Abstraction.Services.ServiceTypes
{
    public interface IServiceTypeService
    {
        Task<IEnumerable<ServiceTypeToReturn>> GetAllServicesTypes();
        Task<ServiceTypeToReturn> GetServiceType(int id);

        Task<string> DeleteServiceType(int id);

        Task<ServiceTypeToReturn> CreateServiceType(ServiceTypeDto createService);
        Task<ServiceTypeToReturn> UpdateServiceType(int id, ServiceTypeDto createService);

    }
}
