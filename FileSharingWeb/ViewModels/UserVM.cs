using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using FileSharingWeb.Resources;

namespace FileSharingWeb.ViewModels
{
    public class UserVM
    {
        [Required(ErrorMessageResourceName = "required_error", ErrorMessageResourceType = typeof(SharedResource))]
        [Display(Name = "username", ResourceType = typeof(SharedResource))]
        public string Username { get; set; }
        [Required(ErrorMessageResourceName = "required_error", ErrorMessageResourceType = typeof(SharedResource))]
        [Display(Name = "first_name", ResourceType = typeof(SharedResource))]
        public string FirstName { get; set; }
        [Required(ErrorMessageResourceName = "required_error", ErrorMessageResourceType = typeof(SharedResource))]
        [Display(Name = "last_name", ResourceType = typeof(SharedResource))]
        public string LastName { get; set; }


        public string Email { get; set; }






    }


    public class ChangePasswordVM
    {
        [Required(ErrorMessageResourceName = "required_error", ErrorMessageResourceType = typeof(SharedResource))]
        [Display(Name = "old_password", ResourceType = typeof(SharedResource))]
        [DataType(DataType.Password, ErrorMessageResourceName = "password_type_error", ErrorMessageResourceType = typeof(SharedResource))]
        public string OldPassword { get; set; }

        [Required(ErrorMessageResourceName = "required_error", ErrorMessageResourceType = typeof(SharedResource))]
        [Display(Name = "new_password", ResourceType = typeof(SharedResource))]
        [DataType(DataType.Password, ErrorMessageResourceName = "password_type_error", ErrorMessageResourceType = typeof(SharedResource))]
        public string NewPassword { get; set; }

        [Required(ErrorMessageResourceName = "required_error", ErrorMessageResourceType = typeof(SharedResource))]
        [Display(Name = "confirm_password", ResourceType = typeof(SharedResource))]
        [DataType(DataType.Password, ErrorMessageResourceName = "password_type_error", ErrorMessageResourceType = typeof(SharedResource))]
        [Compare("NewPassword", ErrorMessageResourceName = "confirm_password_error", ErrorMessageResourceType = typeof(SharedResource))]
        public string ConfirmNewPassword { get; set; }

    }
}