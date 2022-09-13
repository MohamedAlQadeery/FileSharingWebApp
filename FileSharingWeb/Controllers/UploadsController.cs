using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using CloudinaryDotNet.Actions;
using FileSharingWeb.Data;
using FileSharingWeb.Extensions;
using FileSharingWeb.Helpers;
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
        private readonly IUploadsService _uploadsService;
        public UploadsController(IFileUploadService fileUploadService, IUploadsService uploadsService)
        {
            _uploadsService = uploadsService;

            _fileUploadService = fileUploadService;

        }


        public IActionResult Index()
        {
            var uploads = _uploadsService.GetAllUserUploads(User.GetUserId()).ToList();
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

            _uploadsService.Create(upload);
            if (!await _uploadsService.SaveAllAsync()) return View(fileInput);

            TempData["uploaded_success"] = "Your file has been uploaded successfully!";
            return RedirectToAction("Create");
        }


        [HttpGet]
        public async Task<IActionResult> Delete(string publicId)
        {
            var upload = await _uploadsService.GetByPublicIdAsync(publicId);

            var uploadVm = new UploadViewModel
            {
                FileName = upload.FileName,
                ContentType = upload.ContentType,
                uploadUrl = upload.uploadUrl,
                PublicId = upload.PublicId,
                Size = upload.Size,
                CreatedAt = upload.CreatedAt,
                UserId = upload.UserId


            };

            if (upload == null || upload.UserId != User.GetUserId()) return NotFound();
            return View(uploadVm);

        }
        [HttpPost]
        [ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirm(DeleteUploadVM deleteUploadVM)
        {
            var upload = await _uploadsService.GetByPublicIdAsync(deleteUploadVM.PublicId);

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
            _uploadsService.Remove(upload);
            if (!await _uploadsService.SaveAllAsync()) return BadRequest();
            return RedirectToAction("Index");

        }


        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Search([FromQuery] UploadSearchParams searchParams)
        {
            if (searchParams.Term == null)
            {
                TempData["search_empty"] = "File name cant be empty";
                return RedirectToAction("Index", "Home");
            }

            var uploads_query = _uploadsService.GetAllByName(searchParams.Term);

            var uploads = await PagedList<UploadViewModel>.CreateAsync(uploads_query, searchParams.PageIndex, PageSize);
            ViewBag.term = searchParams.Term;
            return View(uploads);
        }


        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Browse(int pageIndex = 1)
        {

            var uploadsQuery = _uploadsService.GetAll();


            var uploads = await PagedList<UploadViewModel>.CreateAsync(uploadsQuery, pageIndex, PageSize);

            return View(uploads);
        }


        [HttpGet]
        public async Task<IActionResult> Download(string fileName)
        {
            var file = await _uploadsService.GetByFileNameAsync(fileName);
            if (file == null)
            {
                return NotFound();
            }
            _uploadsService.IncrementDownload(file);
            if (!await _uploadsService.SaveAllAsync()) return BadRequest();

            return Redirect(file.uploadUrl);

        }

        //Default Page size for pages   
        public int PageSize { get; set; } = 6;
    }
}