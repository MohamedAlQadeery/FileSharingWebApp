using FileSharingWeb.Data;
using FileSharingWeb.Helpers;
using FileSharingWeb.Interfaces.Services;
using FileSharingWeb.Models;
using FileSharingWeb.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Text;

namespace FileSharingWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AppDbContext _ctx;
        private readonly IEmailService _emailService;

        public HomeController(ILogger<HomeController> logger, AppDbContext ctx, IEmailService emailService)
        {
            _emailService = emailService;
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



        [HttpGet]
        public IActionResult Contact()
        {
            return View();
        }

        [HttpPost]
        [ActionName("Contact")]
        public async Task<IActionResult> ContactPost(ContactVM contactVM)
        {
            if (!ModelState.IsValid) return View(contactVM);

            var contact = new Contact()
            {
                FullName = contactVM.FullName,
                Email = contactVM.Email,
                Subject = contactVM.Subject,
                Body = contactVM.Body
            };

            _ctx.Contacts.Add(contact);
            await _ctx.SaveChangesAsync();

            var sb = new StringBuilder();
            sb.AppendLine("<h1>File Sharing - Unread Message</h1>");
            sb.AppendLine($"Name :{contact.FullName}");
            sb.AppendLine($"Email :{contact.Email}");
            sb.AppendLine($"Subject :{contact.Subject}");
            sb.AppendLine($"Message :{contact.Body}");

            //send to our email to inform us
            _emailService.SendEmail(new EmailMessage(new string[] { "moh9amad1@gmail.com" }, contact.Subject, sb.ToString()));
            return RedirectToAction("Contact");


        }


    }
}