using System.ComponentModel.DataAnnotations;

namespace SistemaDeVotacion.web.Models;

public class RegisterViewModel
{
    [Required(ErrorMessage = "Email es requerido")]
    [EmailAddress]
    public string Email { get; set; }

    [Required(ErrorMessage = "La contraseña es obligatoria")]
    [StringLength(100, ErrorMessage = "La {0} debe tener al menos {2} y un máximo de {1} caracteres.", MinimumLength = 6)]
    [DataType(DataType.Password)]
    public string Password { get; set; }
}