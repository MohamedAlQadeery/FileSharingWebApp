using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using FileSharingWeb.Data;
using FileSharingWeb.Extensions;
using FileSharingWeb.Interfaces.Services;
using FileSharingWeb.Models;
using FileSharingWeb.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace FileSharingWeb.Controllers
{
    [Authorize]
    public class UploadsController : Controller
    {
        private readonly IFileUploadService _fileUploadService;
        private readonly AppDbContext _ctx;
        public UploadsController(IFileUploadService fileUploadService, AppDbContext ctx)
        {
            _ctx = ctx;
            _fileUploadService = fileUploadService;

        }
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(FileInputVM fileInput)
        {
            if (!ModelState.IsValid) return View(fileInput);
            var result = await _fileUploadService.UploadFileAsync(fileInput.File);
            if (result.Error != null)
            {
                ModelState.AddModelError("", result.Error.Message);
                return View(fileInput);
            }

            var upload = new Uploads()
            {
                FileName = fileInput.File.FileName,
                ContentType = fileInput.File.ContentType,
                uploadUrl = result.SecureUrl.AbsoluteUri,
                PublicId = result.PublicId,
                UserId = User.GetUserId(),
                Size = fileInput.File.Length
            };

            _ctx.Uploads.Add(upload);
            await _ctx.SaveChangesAsync();

            return View();
        }

    }
}