using Microsoft.EntityFrameworkCore;
using OnlineMobileServices_Models.Models;
namespace OnlineMobileServices_API.Models
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Transactions> Transactions { get; set; }

    }
}
