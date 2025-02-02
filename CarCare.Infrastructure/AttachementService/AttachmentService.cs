using CareCare.Core.Application.Abstraction.Common.Contract.Infrastructure;
using Microsoft.AspNetCore.Http;

namespace CarCare.Infrastructure.AttachementService
{
    public class AttachmentService : IAttachmentService
    {
        private readonly List<string> _allowedExtentions = new() { ".png", ".jpg", ".jpeg" };
        private const int _allowedMaxSize = 2_097_152;
        public async Task<string?> UploadAsynce(IFormFile file, string folderName)
        {
            var extention = Path.GetExtension(file.FileName);

            if (!_allowedExtentions.Contains(extention))
                return null;

            if (file.Length > _allowedMaxSize)
                return null;



            //var folderPath = $"{Directory.GetCurrentDirectory}\\wwwroot\\Files\\{folderName}";

            var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images", folderName);
            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);

            var fileName = $"{Guid.NewGuid()}{extention}"; // must be unique 

            var filePath = Path.Combine(folderPath, fileName); // file location placed

            using var fileStream = new FileStream(filePath, FileMode.Create);


            await file.CopyToAsync(fileStream);

            return fileName;
        }


        public bool Delete(string filePath)
        {
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
                return true;
            }
            return false;

        }
    }
}
