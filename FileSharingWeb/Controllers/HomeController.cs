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

        private readonly IUploadsService _uploadsService;
        private readonly IContactService _contactService;

        public HomeController(ILogger<HomeController> logger, IUploadsService uploadsService, IContactService contactService)
        {
            _contactService = contactService;
            _uploadsService = uploadsService;
            _logger = logger;
        }

        public IActionResult Index()
        {
            var uploads = _uploadsService.GetAll().Take(3);
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

            if (!await _contactService.CreateAsync(contact)) return BadRequest();


            return RedirectToAction("Contact");


        }


    }
}