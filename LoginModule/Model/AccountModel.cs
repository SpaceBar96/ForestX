using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoginModule.Model
{
    public class LoginModel
    {
        [Required]
        [Display(Name = "Email")]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }
    }

    public class LogInDtls
    {
        public bool Verify { get; set; }
        public Guid UserID { get; set; }
        public string UserName { get; set; }
    }

    public class ForgetPassModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Emali { get; set; }
    }

    public class ResetPassModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "New Password")]
        public string Password { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        [Compare("Password", ErrorMessage = "Password and Confirmation Password must match")]
        public string RePassword { get; set; }
    }
}
