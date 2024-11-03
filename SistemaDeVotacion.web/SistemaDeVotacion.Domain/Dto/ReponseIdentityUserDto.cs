using Microsoft.AspNetCore.Identity;

namespace SistemaDeVotacion.Domain.Dto;

public class ReponseIdentityUserDto
{
    public IdentityResult responseIdentity { get; set; }
    public IdentityUser userIdentity { get; set; }
}
