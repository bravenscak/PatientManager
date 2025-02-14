using System.ComponentModel.DataAnnotations;

namespace PatientManagerMvc.Models
{
    public class MedicalRecordViewModel
    {
        public long Id { get; set; }

        [Required(ErrorMessage = "Please enter a diagnosis")]
        public string Diagnosis { get; set; }

        [Required(ErrorMessage = "Please enter a start date")]
        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public long PatientId { get; set; }
    }
}
