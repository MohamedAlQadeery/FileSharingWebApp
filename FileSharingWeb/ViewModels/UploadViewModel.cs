using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FileSharingWeb.ViewModels
{
    public class UploadViewModel
    {
        public string FileName { get; set; }
        public string ContentType { get; set; }
        public decimal Size { get; set; }
        public string uploadUrl { get; set; }

        public string PublicId { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}