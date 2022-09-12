using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FileSharingWeb.Helpers
{
    public class UploadSearchParams
    {
        public string Term { get; set; }
        public int PageIndex { get; set; } = 1;
    }
}