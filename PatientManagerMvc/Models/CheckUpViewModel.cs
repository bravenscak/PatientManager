using Microsoft.AspNetCore.Mvc.Rendering;
using PatientManagerClassLibrary.Enums;
using System.ComponentModel.DataAnnotations;

namespace PatientManagerMvc.Models
{
    public class CheckUpViewModel
    {
        public long Id { get; set; }

        [Required(ErrorMessage = "Please enter a date")]
        public DateTime Date { get; set; }

        [Required(ErrorMessage = "Please enter a type")]
        public CheckUpType Type { get; set; }

        [Required(ErrorMessage = "Please select a patient")]
        public long PatientId { get; set; }

        public List<SelectListItem> Patients { get; set; }
    }
}
