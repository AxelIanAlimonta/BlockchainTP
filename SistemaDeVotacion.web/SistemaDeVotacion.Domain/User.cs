using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace SistemaDeVotacion.Domain;

public class User
{
    [Key]
    public string Id { get; set; }
    public IdentityUser IdentityUser { get; set; }

    public string? WalletAddress { get; set; }

    public string Email => IdentityUser?.Email;
}