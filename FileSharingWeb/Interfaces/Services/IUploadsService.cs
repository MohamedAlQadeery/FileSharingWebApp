using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FileSharingWeb.Models;
using FileSharingWeb.ViewModels;

namespace FileSharingWeb.Interfaces.Services
{
    public interface IUploadsService
    {
        void Create(Uploads upload);
        void Remove(Uploads upload);
        IQueryable<UploadViewModel> GetAll();
        IQueryable<UploadViewModel> GetAllByName(string term);
        IQueryable<UploadViewModel> GetAllUserUploads(string userId);

        Task<Uploads> FindByIdAsync(string id);
        Task<Uploads> GetByPublicIdAsync(string publicId);
        Task<Uploads> GetByFileNameAsync(string fileName);
        void IncrementDownload(Uploads upload);

        Task<bool> SaveAllAsync();
    }
}