using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Web.Models.Doctor
{
    public class DoctorAddMedicineViewModel
    {
        [Required(ErrorMessage = "Medicine Name Required")]
        public string MedicineName { get; set; }

        [Required(ErrorMessage = "Medicine Frequency Required")]
        public int medicineDefaultFrequency { get; set; }

        [Required(ErrorMessage = "Medicine Image Required")]
        public IFormFile medicineImage { get; set; }
    }
}
