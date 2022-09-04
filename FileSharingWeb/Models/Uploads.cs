using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FileSharingWeb.Models
{
    public class Uploads
    {
        public Uploads()
        {
            Id = Guid.NewGuid().ToString();

        }
        public string Id { get; set; }
        public string FileName { get; set; }
        public string ContentType { get; set; }
        public decimal Size { get; set; }
        public string UserId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? UpdatedAt { get; set; }

        public string uploadUrl { get; set; }

        public string PublicId { get; set; }

    }
}