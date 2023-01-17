using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Web.Models.Doctor
{
    public class DoctorAddMedicineViewModel
    {
        [Required(ErrorMessage = "Medicine Name Required")]
        public string MedicineName { get; set; }

        [Required(ErrorMessage = "Medicine Frequency Required")]
        public int medicineDefaultFrequency { get; set; } 
        
        public string medicineDefaultTimeList { get; set; }

        [Required(ErrorMessage = "Medicine Image Required")]
        public IFormFile medicineImage { get; set; }
    }
}
