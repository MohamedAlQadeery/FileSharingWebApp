using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using FileSharingWeb.Resources;

namespace FileSharingWeb.ViewModels
{
    public class LoginVM
    {
        [EmailAddress(ErrorMessageResourceName = "email_error", ErrorMessageResourceType = typeof(SharedResource))]
        [Required(ErrorMessageResourceName = "required_error", ErrorMessageResourceType = typeof(SharedResource))]
        [Display(Name = "email", ResourceType = typeof(SharedResource))]
        public string Email { get; set; }
        [Required(ErrorMessageResourceName = "required_error", ErrorMessageResourceType = typeof(SharedResource))]
        [Display(Name = "password", ResourceType = typeof(SharedResource))]
        [DataType(DataType.Password,ErrorMessageResourceName ="password_type_error",ErrorMessageResourceType =typeof(SharedResource))]
        public string Password { get; set; }
    }
}