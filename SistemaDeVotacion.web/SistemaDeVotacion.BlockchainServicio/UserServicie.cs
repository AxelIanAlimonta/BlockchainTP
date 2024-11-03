using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SistemaDeVotacion.Domain;
using SistemaDeVotacion.Domain.Context;
using SistemaDeVotacion.Domain.Dto;
using System.Security.Claims;


namespace SistemaDeVotacion.BlockchainServicio;

public class UserService
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly BlockchainDbContext _context;

    public UserService(UserManager<IdentityUser> userManager, BlockchainDbContext context)
    {
        _userManager = userManager;
        _context = context;
    }

    public async Task<bool> AddWalletAsync(IdentityUser identityUser, string walletAddress)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.IdentityUser.Id == identityUser.Id);

        if (user == null)
        {
            return false; // El usuario no existe
        }

        user.WalletAddress = walletAddress;

        await _context.SaveChangesAsync();

        return true; // Indicar que se completó exitosamente
    }

    public async Task<User> GetUserWithWalletAsync(ClaimsPrincipal user)
    {
        var identityUser = await _userManager.GetUserAsync(user);
        if (identityUser == null)
        {
            return null; // No hay usuario autenticado
        }

        // Buscar el usuario con su wallet asociada usando el Id del IdentityUser
        var userWithWallet = await _context.Users
            .FirstOrDefaultAsync(u => u.IdentityUser.Id == identityUser.Id);
        
        return userWithWallet;
    }

    public async Task<ReponseIdentityUserDto> RegisterUserAsync(RegisterUserDto model)
    {
        var user = new IdentityUser
        {
            UserName = model.Email,
            Email = model.Email
        };

        var result = await _userManager.CreateAsync(user, model.Password);

        if (result.Succeeded)
        {
            // Crear el User con WalletAddress en null
            var userWithWallet = new User
            {
                IdentityUser = user,
                WalletAddress = null
            };

            // Agregar el nuevo usuario al contexto
            await _context.Users.AddAsync(userWithWallet);
            await _context.SaveChangesAsync(); // Guardar cambios en la base de datos
        }

        return new ReponseIdentityUserDto{
            responseIdentity = result,
            userIdentity = user,
        };
    }
}
