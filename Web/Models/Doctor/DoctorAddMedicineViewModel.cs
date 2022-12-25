using System.ComponentModel.DataAnnotations;

namespace Web.Models.Doctor
{
    public class DoctorAddMedicineViewModel
    {
        [Required(ErrorMessage = "Required")]
        public string MedicineName { get; set; }
    }
}
