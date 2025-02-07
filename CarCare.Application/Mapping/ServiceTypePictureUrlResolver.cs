using AutoMapper;
using CarCare.Core.Domain.Entities.ServiceTypes;
using CareCare.Core.Application.Abstraction.Models.ServiceTypes;
using Microsoft.Extensions.Configuration;

namespace CarCare.Core.Application.Mapping
{
    public class ServiceTypePictureUrlResolver(IConfiguration configuration) : IValueResolver<ServiceType, ServiceTypeToReturn, string?>
    {
        public string? Resolve(ServiceType source, ServiceTypeToReturn destination, string? destMember, ResolutionContext context)
        {
            if (!string.IsNullOrEmpty(source.PictureUrl))
            {
                return $"{configuration["Urls:ApiBaseUrl"]}/{source.PictureUrl}";
            }
            return string.Empty;
        }
    }
}
