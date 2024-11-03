using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace SistemaDeVotacion.Domain.Context;

public class BlockchainDbContext : IdentityDbContext
{
    public BlockchainDbContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(u => u.Id); // Clave primaria

            // Relación con IdentityUser
            entity.HasOne(u => u.IdentityUser)
                  .WithOne()
                  .HasForeignKey<User>(u => u.Id);
        });
    }
}

