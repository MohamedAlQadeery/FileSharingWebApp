using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using FileSharingWeb.Resources;
using Microsoft.AspNetCore.Mvc;

namespace FileSharingWeb.ViewModels
{
    public class RegisterVM
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

        [EmailAddress(ErrorMessageResourceName = "email_error", ErrorMessageResourceType = typeof(SharedResource))]
        [Required(ErrorMessageResourceName = "required_error", ErrorMessageResourceType = typeof(SharedResource))]
        [Display(Name = "email", ResourceType = typeof(SharedResource))]
        public string Email { get; set; }

        [Required(ErrorMessageResourceName = "required_error", ErrorMessageResourceType = typeof(SharedResource))]
        [Display(Name = "password", ResourceType = typeof(SharedResource))]
        [DataType(DataType.Password, ErrorMessageResourceName = "password_type_error", ErrorMessageResourceType = typeof(SharedResource))]
        public string Password { get; set; }

        [Required(ErrorMessageResourceName = "required_error", ErrorMessageResourceType = typeof(SharedResource))]
        [Display(Name = "confirm_password", ResourceType = typeof(SharedResource))]
        [DataType(DataType.Password, ErrorMessageResourceName = "password_type_error", ErrorMessageResourceType = typeof(SharedResource))]
        [Compare("Password", ErrorMessageResourceName = "confirm_password_error", ErrorMessageResourceType = typeof(SharedResource))]
        public string ConfirmPassword { get; set; }
    }
}