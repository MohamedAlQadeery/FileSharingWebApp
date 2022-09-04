using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FileSharingWeb.ViewModels
{
    public class FileInputVM
    {
        [Required]
        [DataType(DataType.Upload)]
        public IFormFile File { get; set; }
    }
}