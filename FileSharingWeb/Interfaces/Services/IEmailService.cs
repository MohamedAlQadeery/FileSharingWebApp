using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FileSharingWeb.Helpers;

namespace FileSharingWeb.Interfaces.Services
{
    public interface IEmailService
    {
        void SendEmail(EmailMessage message);
    }
}