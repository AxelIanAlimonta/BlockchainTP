using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace SistemaDeVotacion.web.Context
{
    public class BlockchainDbContext : IdentityDbContext
    {
        public BlockchainDbContext(DbContextOptions options) : base(options)
        {
        }
    }
}
