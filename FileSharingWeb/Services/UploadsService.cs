using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FileSharingWeb.Data;
using FileSharingWeb.Interfaces.Services;
using FileSharingWeb.Models;
using FileSharingWeb.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace FileSharingWeb.Services
{
    public class UploadsService : IUploadsService
    {
        private readonly AppDbContext _ctx;
        public UploadsService(AppDbContext ctx)
        {
            _ctx = ctx;

        }



        public Task<Uploads> FindByIdAsync(string id)
        {
            throw new NotImplementedException();
        }

        public IQueryable<UploadViewModel> GetAll()
        {
            return _ctx.Uploads.Select(u => new UploadViewModel
            {
                FileName = u.FileName,
                ContentType = u.ContentType,
                uploadUrl = u.uploadUrl,
                PublicId = u.PublicId,
                Size = u.Size,
                CreatedAt = u.CreatedAt,
                DownloadCount = u.DownloadCount


            }).OrderByDescending(u => u.DownloadCount).AsQueryable();
        }

        public IQueryable<UploadViewModel> GetAllByName(string term)
        {
            return _ctx.Uploads.Where(u => u.FileName.Contains(term)).Select(u => new UploadViewModel
            {
                FileName = u.FileName,
                ContentType = u.ContentType,
                uploadUrl = u.uploadUrl,
                PublicId = u.PublicId,
                Size = u.Size,
                CreatedAt = u.CreatedAt,
                DownloadCount = u.DownloadCount

            }).OrderByDescending(u => u.DownloadCount).AsQueryable();
        }

        public async Task<Uploads> GetByPublicIdAsync(string publicId)
        {
            return await _ctx.Uploads.Where(u => u.PublicId == publicId).FirstOrDefaultAsync();


        }




        public async Task<bool> SaveAllAsync()
        {
            return await _ctx.SaveChangesAsync() > 0;
        }



        public void Create(Uploads upload)
        {
            _ctx.Uploads.Add(upload);
        }

        public void Remove(Uploads upload)
        {
            _ctx.Uploads.Remove(upload);

        }

        public void IncrementDownload(Uploads upload)
        {
            upload.DownloadCount++;
            _ctx.Uploads.Update(upload);
        }

        public async Task<Uploads> GetByFileNameAsync(string fileName)
        {
            return await _ctx.Uploads.FirstOrDefaultAsync(u => u.FileName == fileName);
        }

        public IQueryable<UploadViewModel> GetAllUserUploads(string userId)
        {
            return _ctx.Uploads.Where(u => u.UserId == userId).Select(u => new UploadViewModel
            {
                FileName = u.FileName,
                ContentType = u.ContentType,
                uploadUrl = u.uploadUrl,
                PublicId = u.PublicId,
                Size = u.Size,
                CreatedAt = u.CreatedAt,
                DownloadCount = u.DownloadCount


            }).OrderByDescending(u => u.CreatedAt).AsQueryable();
        }
    }
}