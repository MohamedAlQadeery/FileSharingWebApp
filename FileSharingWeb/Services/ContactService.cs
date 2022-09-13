using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FileSharingWeb.Data;
using FileSharingWeb.Helpers;
using FileSharingWeb.Interfaces.Services;
using FileSharingWeb.Models;

namespace FileSharingWeb.Services
{
    public class ContactService : IContactService
    {
        private readonly AppDbContext _ctx;
        private readonly IEmailService _emailService;
        public ContactService(AppDbContext ctx, IEmailService emailService)
        {
            _emailService = emailService;
            _ctx = ctx;

        }
        public async Task<bool> CreateAsync(Contact contact)
        {
            _ctx.Contacts.Add(contact);

            if (!await SaveAllAsync()) return false;

            var sb = new StringBuilder();
            sb.AppendLine("<h1>File Sharing - Unread Message</h1>");
            sb.AppendLine($"Name :{contact.FullName}");
            sb.AppendLine($"Email :{contact.Email}");
            sb.AppendLine($"Subject :{contact.Subject}");
            sb.AppendLine($"Message :{contact.Body}");

            //send to our email to inform us
            _emailService.SendEmail(new EmailMessage(new string[] { "moh9amad1@gmail.com" }, contact.Subject, sb.ToString()));
            return true;
        }

        public async Task<bool> SaveAllAsync()
        {
            return await _ctx.SaveChangesAsync() > 0;
        }
    }
}