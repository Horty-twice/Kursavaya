using Microsoft.EntityFrameworkCore;
namespace LR4.Clients.Models
{
    public class ClientContext : DbContext
    {
        public DbSet<Client> Clients { get; set; } = null!;
        public ClientContext(DbContextOptions<ClientContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }
    }
}
