using System.ComponentModel.DataAnnotations;

namespace PatientManagerMvc.Models
{
    public class PatientViewModel
    {
        public long Id { get; set; }

        [Required(ErrorMessage = "Please enter a first name")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Please enter a last name")]
        public string LastName { get; set; }

        [Required(ErrorMessage= "Please enter sex")]
        public string Sex{ get; set; }

        [Required(ErrorMessage = "Please enter an OIB")]
        public string Oib { get; set; }

        [Required(ErrorMessage = "Please enter a date of birth")]
        public DateTime DateOfBirth { get; set; }

        public ICollection<MedicalRecordViewModel> MedicalRecords { get; set; } = new List<MedicalRecordViewModel>();

        public ICollection<PrescriptionViewModel> Prescriptions { get; set; } = new List<PrescriptionViewModel>();

        public ICollection<CheckUpViewModel> CheckUps { get; set; } = new List<CheckUpViewModel>();
    }
}
