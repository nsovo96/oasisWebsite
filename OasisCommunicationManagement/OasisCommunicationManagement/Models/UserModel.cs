using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OasisCommunicationManagement.Models
{
    public class UserModel
    {
        public int id { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Enter a valid name", MinimumLength = 3)]
        [DataType(DataType.Text)]
        [Display(Name = "Name")]
        public string FullNames { get; set; }
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 3)]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email")]
        public string Email { get; set; }
        public string UserRole { get; set; }
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 3)]
        [DataType(DataType.Password)]
        [Display(Name = "Enter password")]
        public string Password { get; set; }

        public string isActive { get; set; }
    }

    public class createuser: UserModel
    {

    }
    public class login : UserModel
    {

        public string Email { get; set; }
        public string Password { get; set; }

    }

    public class Uthenitcate 
    {
        public string status { get; set; }
    }
}