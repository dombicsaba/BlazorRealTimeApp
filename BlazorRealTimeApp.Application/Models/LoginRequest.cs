using System.ComponentModel.DataAnnotations;

namespace BlazorRealTimeApp.Application.Models
{
    public class LoginRequest
    {
        [Required(ErrorMessage = "Felhaszn�l�n�v k�telez�")]
        public string Username { get; set; } = string.Empty;

        [Required(ErrorMessage = "Jelsz� k�telez�")]
        public string Password { get; set; } = string.Empty;
    }
}
