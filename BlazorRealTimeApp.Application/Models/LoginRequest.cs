using System.ComponentModel.DataAnnotations;

namespace BlazorRealTimeApp.Application.Models
{
    public class LoginRequest
    {
        [Required(ErrorMessage = "Felhasználónév kötelezõ")]
        public string Username { get; set; } = string.Empty;

        [Required(ErrorMessage = "Jelszó kötelezõ")]
        public string Password { get; set; } = string.Empty;
    }
}
