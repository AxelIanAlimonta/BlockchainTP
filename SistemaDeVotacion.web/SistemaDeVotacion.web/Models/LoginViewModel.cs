using System.ComponentModel.DataAnnotations;

namespace SistemaDeVotacion.web.Models;

public class LoginViewModel
{
    [Required(ErrorMessage = "Este campo es requerido")]
    [EmailAddress]
    public string Email { get; set; }

    [Required(ErrorMessage = "Este campo es requerido")]
    [DataType(DataType.Password)]
    public string Password { get; set; }
}