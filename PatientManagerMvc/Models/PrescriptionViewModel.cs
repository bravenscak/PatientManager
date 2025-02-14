using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace PatientManagerMvc.Models
{
    public class PrescriptionViewModel
    {
        public long Id { get; set; }

        [Required(ErrorMessage = "Medication is required")]
        public string Medication { get; set; }

        [Required(ErrorMessage = "Dosage is required")]
        public string Dosage { get; set; }

        [Required(ErrorMessage = "Start date is required")]
        public DateTime StartDate { get; set; }

        public long PatientId { get; set; }

        public IEnumerable<SelectListItem> Patients { get; set; }

    }
}
