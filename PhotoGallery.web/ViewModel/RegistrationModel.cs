using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PhotoGallery.web.ViewModel
{
    public class RegistrationModel : IValidatableObject
    {
        //Extra properties for Registration
        [Required, RegularExpression("[a-zA-Z]+")]
        public string Firstname { get; set; }

        [Required, RegularExpression("[a-zA-Z]+")]
        public string Lastname { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string ConfirmPassword { get; set; }

        public string ReturnUrl { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            //var result = new List<ValidationResult>();

            if (ConfirmPassword.Equals(Password) == false)
                yield return new ValidationResult("Passwords do not match", new List<string> { ConfirmPassword });
        }
    }
}