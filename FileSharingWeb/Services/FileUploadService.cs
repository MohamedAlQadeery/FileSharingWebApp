using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using FileSharingWeb.Helpers;
using FileSharingWeb.Interfaces.Services;
using Microsoft.Extensions.Options;

namespace FileSharingWeb.Services
{
    public class FileUploadService : IFileUploadService
    {
        private readonly Cloudinary _cloudinary;

        public FileUploadService(IOptions<CloudinarySettings> config)
        {
            var account = new Account(config.Value.CloudName, config.Value.ApiKey, config.Value.ApiSecret);
            _cloudinary = new Cloudinary(account);
        }

        public Task<DeletionParams> DeleteFileAsync(string publicId)
        {
            throw new NotImplementedException();
        }

        public async Task<ImageUploadResult> UploadFileAsync(IFormFile file)
        {
            var uploadResult = new ImageUploadResult();
            if (file.Length > 0)
            {
                using var stream = file.OpenReadStream();
                var uploadParams = new ImageUploadParams
                {
                    File = new FileDescription(file.FileName, stream),
                    Transformation = new Transformation().Height(500).Width(500).Crop("fill").Gravity("face"),
                    Folder = "FileSharingApp"

                };

                uploadResult = await _cloudinary.UploadAsync(uploadParams);
            }
            return uploadResult;
        }



        public async Task<VideoUploadResult> UploadVideoAsync(IFormFile video)
        {
            var uploadResult = new VideoUploadResult();

            if (video.Length > 0)
            {
                using var stream = video.OpenReadStream();
                var uploadParams = new VideoUploadParams()
                {
                    File = new FileDescription(video.FileName, stream),
                    Folder = "FileSharingApp",
                    Transformation = new Transformation().Height(360).Width(480).Quality(50).Crop("fill")
                };

                uploadResult = await _cloudinary.UploadAsync(uploadParams);
            }
            return uploadResult;
        }
    }
}