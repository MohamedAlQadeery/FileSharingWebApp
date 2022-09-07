using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using CloudinaryDotNet.Actions;
using FileSharingWeb.Data;
using FileSharingWeb.Extensions;
using FileSharingWeb.Interfaces.Services;
using FileSharingWeb.Models;
using FileSharingWeb.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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


        public IActionResult Index()
        {
            var uploads = _ctx.Uploads.Where(u => u.UserId == User.GetUserId()).Select(u => new UploadViewModel
            {
                FileName = u.FileName,
                ContentType = u.ContentType,
                uploadUrl = u.uploadUrl,
                PublicId = u.PublicId,
                Size = u.Size,
                CreatedAt = u.CreatedAt,
                DownloadCount = u.DownloadCount


            }).OrderByDescending(u => u.CreatedAt).ToList();
            return View(uploads);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(FileInputVM fileInput)
        {
            if (!ModelState.IsValid) return View(fileInput);

            var result = new RawUploadResult();
            if (fileInput.File.ContentType.Contains("video"))
            {
                result = await _fileUploadService.UploadVideoAsync(fileInput.File);
            }
            else
            {
                result = await _fileUploadService.UploadFileAsync(fileInput.File);
            }

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
            TempData["uploaded_success"] = "Your file has been uploaded successfully!";
            return RedirectToAction("Create");
        }


        [HttpGet]
        public async Task<IActionResult> Delete(string publicId)
        {
            var upload = await _ctx.Uploads.Where(u => u.PublicId == publicId).Select(u => new UploadViewModel
            {
                FileName = u.FileName,
                ContentType = u.ContentType,
                uploadUrl = u.uploadUrl,
                PublicId = u.PublicId,
                Size = u.Size,
                CreatedAt = u.CreatedAt,
                UserId = u.UserId


            }).FirstOrDefaultAsync();

            if (upload == null || upload.UserId != User.GetUserId()) return NotFound();
            return View(upload);

        }
        [HttpPost]
        [ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirm(DeleteUploadVM deleteUploadVM)
        {
            var upload = _ctx.Uploads.Where(u => u.PublicId == deleteUploadVM.PublicId).FirstOrDefault();

            if (upload == null || upload.UserId != User.GetUserId())
            {
                return NotFound();
            }

            var resourceType = deleteUploadVM.ResourceType.Contains("video") ? ResourceType.Video : ResourceType.Image;
            var result = await _fileUploadService.DeleteFileAsync(deleteUploadVM.PublicId, resourceType);
            if (result.Error != null)
            {
                ModelState.AddModelError("", "Something went wrong");
                return RedirectToAction("Index");

            }
            _ctx.Uploads.Remove(upload);
            await _ctx.SaveChangesAsync();
            return RedirectToAction("Index");

        }


        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Search(string term)
        {
            if (term == null)
            {
                TempData["search_empty"] = "File name cant be empty";
                return RedirectToAction("Index", "Home");
            }

            var uploads = await _ctx.Uploads.Where(u => u.FileName.Contains(term)).Select(u => new UploadViewModel
            {
                FileName = u.FileName,
                ContentType = u.ContentType,
                uploadUrl = u.uploadUrl,
                PublicId = u.PublicId,
                Size = u.Size,
                CreatedAt = u.CreatedAt,
                DownloadCount = u.DownloadCount

            }).OrderByDescending(u => u.DownloadCount).ToListAsync();
            ViewBag.term = term;
            return View(uploads);
        }


        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Browse()
        {
            var uploads = await _ctx.Uploads.Select(u => new UploadViewModel
            {
                FileName = u.FileName,
                ContentType = u.ContentType,
                uploadUrl = u.uploadUrl,
                PublicId = u.PublicId,
                Size = u.Size,
                CreatedAt = u.CreatedAt,
                DownloadCount = u.DownloadCount


            }).OrderByDescending(u => u.DownloadCount).ToListAsync();

            return View(uploads);
        }


        [HttpGet]
        public async Task<IActionResult> Download(string fileName)
        {
            var file = await _ctx.Uploads.FirstOrDefaultAsync(u => u.FileName == fileName);
            if (file == null)
            {
                return NotFound();
            }
            file.DownloadCount++;
            _ctx.Uploads.Update(file);
            await _ctx.SaveChangesAsync();
            return Redirect(file.uploadUrl);

        }
    }
}