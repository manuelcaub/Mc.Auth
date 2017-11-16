using Mc.Auth.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Mc.Auth.Database
{
    public class ApplicationContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasKey(x => x.Id);
        }
    }
}
