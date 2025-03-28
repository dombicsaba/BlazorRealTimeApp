using System.ComponentModel.DataAnnotations;

namespace BlazorRealTimeApp.Application.Models
{
    public class LoginRequest
    {
        [Required(ErrorMessage = "Felhasználónév kötelező")]
        public string Username { get; set; } = string.Empty;

        [Required(ErrorMessage = "Jelszó kötelező")]
        public string Password { get; set; } = string.Empty;
    }
}
