using System;
using System.ComponentModel.DataAnnotations;

namespace Web.Models.Home
{
    public class RegisterAccountViewModel
    {
        [Required(ErrorMessage = "Required"), EmailAddress(ErrorMessage = "Please enter a valid email address")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Required"), MinLength(5, ErrorMessage = "MinLength")]
        public string Password { get; set; }
        [Required(ErrorMessage = "Required"), Compare("Password", ErrorMessage = "Compare")]
        public string PasswordRepeat { get; set; }
    }
}

