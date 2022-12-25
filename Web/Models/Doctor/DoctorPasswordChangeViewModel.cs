using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace Web.Models.Doctor
{
    public class DoctorPasswordChangeViewModel
    {
        [Required(ErrorMessage = "Required")]
        public string OldPassword { get; set; }
        [Required(ErrorMessage = "Required")]
        public string NewPassword { get; set; }
        [Compare(nameof(NewPassword), ErrorMessage = "Passwords don't match")]
        public string NewPasswordRepeat { get; set; }
    }
}
