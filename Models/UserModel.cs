using System.ComponentModel.DataAnnotations;

namespace FjernvarmeMaalingApp.Models;

public class UserModel
{
    [Required(ErrorMessage = "Brugernavn er påkrævet")]
    public string? Username { get; set; }

    [Required(ErrorMessage = "Adgangskode er påkrævet")]
    public string? Password { get; set; }

    public string? Response { get; set; }

    public void ResetInstance()
    {
        Username = string.Empty;
        Password = string.Empty;
    }
}