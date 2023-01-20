using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Web.Models.Doctor
{
    public class EditMedicineViewModel
    {
        public int MedicineId { get; set; }
        [Required(ErrorMessage = "Medicine name is required")]
        public string MedicineName { get; set; }
        [Required(ErrorMessage = "Medicine frequency is required")]
        public int MedicineDefaultFrequency { get; set; }
        public IFormFile MedicineImage { get; set; }
        public string ImagePath { get; set; }
        public List<string> MedicineDefaultTimeList { get; set; }
    }
}
