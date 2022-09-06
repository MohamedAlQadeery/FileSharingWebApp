using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FileSharingWeb.ViewModels
{
    public class UploadViewModel
    {
        public string UserId { get; set; }
        public string FileName { get; set; }
        public string ContentType { get; set; }
        public decimal Size { get; set; }
        public string uploadUrl { get; set; }

        public string PublicId { get; set; }
        public DateTime CreatedAt { get; set; }
        public long DownloadCount { get; set; }
    }


    public class DeleteUploadVM
    {
        public string PublicId { get; set; }
        public string ResourceType { get; set; }

    }
}