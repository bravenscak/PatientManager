using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace PatientManagerMvc.Models
{
    public class MedicalFileViewModel
    {
        public long Id { get; set; }

        [Required(ErrorMessage = "File path is required")]
        public string ObjectId { get; set; }

        public long CheckUpId{ get; set; }

        [ValidateNever]
        public List<SelectListItem> CheckUps { get; set; }

    }
}
