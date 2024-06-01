using System.ComponentModel.DataAnnotations;

namespace NotesApp.ViewModels.User
{
    public class LoginFormModel
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; } = null!;
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; } = null!;
        public string? ReturnUrl { get; set; }
    }
}
