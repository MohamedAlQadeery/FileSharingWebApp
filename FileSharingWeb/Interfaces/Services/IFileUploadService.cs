using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CloudinaryDotNet.Actions;

namespace FileSharingWeb.Interfaces.Services
{
    public interface IFileUploadService
    {
        Task<ImageUploadResult> UploadFileAsync(IFormFile file);
        Task<VideoUploadResult> UploadVideoAsync(IFormFile video);

        Task<DeletionParams> DeleteFileAsync(string publicId);
    }
}