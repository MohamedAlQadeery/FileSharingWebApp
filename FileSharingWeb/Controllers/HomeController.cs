using FileSharingWeb.Data;
using FileSharingWeb.Models;
using FileSharingWeb.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace FileSharingWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AppDbContext _ctx;

        public HomeController(ILogger<HomeController> logger, AppDbContext ctx)
        {
            _logger = logger;
            _ctx = ctx;
        }

        public IActionResult Index()
        {
            var uploads = _ctx.Uploads.OrderByDescending(u => u.DownloadCount).Select(u => new UploadViewModel
            {
                FileName = u.FileName,
                uploadUrl = u.uploadUrl,
                PublicId = u.PublicId,
                DownloadCount = u.DownloadCount,
                ContentType = u.ContentType,


            }).Take(3);
            ViewBag.popularDownloads = uploads;
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}