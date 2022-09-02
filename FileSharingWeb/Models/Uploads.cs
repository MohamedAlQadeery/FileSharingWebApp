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
        public string Size { get; set; }
        public string UserId { get; set; }
    }
}