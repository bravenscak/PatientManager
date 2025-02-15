using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using PatientManagerClassLibrary.Models;
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

        [ValidateNever]
        public IEnumerable<SelectListItem> Patients { get; set; }

        [ValidateNever]
        public Patient Patient { get; set; } 
    }


}
