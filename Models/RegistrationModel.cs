using System.ComponentModel.DataAnnotations;

namespace Online_Shop.Models
{
    public class RegistrationModel
    {
        [Required(ErrorMessage = "User Name is required")]
        public string Username { get; set; } = string.Empty;

        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; } = string.Empty;

        [EmailAddress]
        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; } = string.Empty;
    }
}
