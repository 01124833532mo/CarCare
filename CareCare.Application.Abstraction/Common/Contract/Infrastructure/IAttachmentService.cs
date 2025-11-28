using Microsoft.AspNetCore.Http;

namespace CareCare.Core.Application.Abstraction.Common.Contract.Infrastructure
{
    public interface IAttachmentService
    {
        Task<string?> UploadAsynce(IFormFile file, string folderName);



        bool Delete(string filePath);
    }
}
