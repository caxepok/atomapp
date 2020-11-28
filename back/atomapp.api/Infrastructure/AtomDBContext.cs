using atomapp.api.Models.Database;
using Microsoft.EntityFrameworkCore;

namespace atomapp.api.Infrastructure
{
    public class AtomDBContext : DbContext
    {
        public DbSet<Tsk> Tsks { get; set; }
        public DbSet<TskComment> TskComments { get; set; }
        public DbSet<TskTemplate> TskTemplates { get; set; }
        public DbSet<Worker> Workers { get; set; }
        public DbSet<Workplace> Workplaces { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseNpgsql("host=localhost;database=atomapp;user id=postgres;password=System64");
    }
}