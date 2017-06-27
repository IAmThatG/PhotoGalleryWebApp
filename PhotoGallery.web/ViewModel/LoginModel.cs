using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace PhotoGallery.web.ViewModel
{
    public class LoginModel
    {
        //Login properties
        [Required, EmailAddress]
        public string Email { get; set; }

        [Required, MinLength(5, ErrorMessage ="Passord is too short")]
        public string Password { get; set; }

        public bool RememberMe { get; set; }
        public string ReturnUrl { get; set; }

        public RegistrationModel regModel { get; set; }
    }
}
