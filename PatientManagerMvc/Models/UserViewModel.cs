using System.ComponentModel.DataAnnotations;

namespace PatientManagerMvc.Models
{
    public class UserViewModel
    {
        public long Id { get; set; }

        [Required(ErrorMessage = "Please enter a username")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Please enter a password")]
        [StringLength(256, MinimumLength = 8, ErrorMessage = "Password should be at least 8 characters long")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Please enter a first name")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "First name should be between 2 and 50 characters long")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Please enter a last name")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Last name should be between 2 and 50 characters long")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Please enter an email address")]
        [EmailAddress(ErrorMessage = "Provide a correct e-mail address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Please enter a phone number")]
        [Phone(ErrorMessage = "Provide a correct phone number")]
        public string PhoneNumber { get; set; }
    }
}
