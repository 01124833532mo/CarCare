using AutoMapper;
using CarCare.Core.Domain.Contracts.Persistence;
using CarCare.Core.Domain.Entities.ServiceTypes;
using CarCare.Shared.ErrorModoule.Exeptions;
using CareCare.Core.Application.Abstraction.Common.Contract.Infrastructure;
using CareCare.Core.Application.Abstraction.Models.ServiceTypes;
using CareCare.Core.Application.Abstraction.Services.ServiceTypes;

namespace CarCare.Core.Application.Services.ServiceTypes
{
    public class ServiceTypeService(IUnitOfWork _unitOfWork, IMapper _mapper, IAttachmentService attachmentService) : IServiceTypeService
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

        public async Task<ServiceTypeToReturn> CreateServiceType(ServiceTypeDto createService)
        {
            var servicetype = new ServiceType()
            {
                Name = createService.Name,
                Description = createService.Description,
                NormalizedName = createService.Name.ToUpper()
            };

            // Upload the image if PictureUrl is provided
            if (createService.PictureUrl is not null)
            {
                var uploadedImageUrl = await attachmentService.UploadAsynce(createService.PictureUrl, "ServiceTypes");

                // Check if the upload was successful
                if (uploadedImageUrl is not null)
                {
                    servicetype.PictureUrl = uploadedImageUrl;
                }
                else
                {
                    // Handle the case where the upload fails (e.g., invalid file type or size)
                    servicetype.PictureUrl = null; // or set a default image URL
                }
            }

            // Add the ServiceType to the repository
            await _unitOfWork.GetRepository<ServiceType, int>().AddAsync(servicetype);

            // Save changes to the database
            var result = await _unitOfWork.CompleteAsync() > 0;

            // Prepare the response object
            var returnedservice = new ServiceTypeToReturn()
            {
                Id = servicetype.Id,
                Name = servicetype.Name,
                Description = servicetype.Description,
                PictureUrl = servicetype.PictureUrl // This will be null if no image was uploaded or if the upload failed
            };

            return returnedservice;

        }

        public async Task<ServiceTypeToReturn> UpdateServiceType(int id, ServiceTypeDto updateDto)
        {
            var servicetype = await _unitOfWork.GetRepository<ServiceType, int>().GetAsync(id);
            if (servicetype is null) throw new NotFoundExeption("No Service Type Exsist Fot This Id", nameof(id));

            servicetype.Name = updateDto.Name;
            servicetype.Description = updateDto.Description;
            servicetype.NormalizedName = updateDto.Name.ToUpper();
            if (updateDto.PictureUrl is not null)
            {
                servicetype.PictureUrl = await attachmentService.UploadAsynce(updateDto.PictureUrl, "ServiceTypes");
            }

            _unitOfWork.GetRepository<ServiceType, int>().Update(servicetype);
            var result = await _unitOfWork.CompleteAsync() > 0;
            var returnedservice = new ServiceTypeToReturn()
            {
                Id = servicetype.Id,
                Name = servicetype.Name,
                Description = servicetype.Description,
                PictureUrl = servicetype.PictureUrl,

            };
            return returnedservice;
        }
    }
}
