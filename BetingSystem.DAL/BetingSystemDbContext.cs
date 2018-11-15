using BetingSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace BetingSystem.DAL
{
    public class BetingSystemDbContext : DbContext
    {
        public DbSet<BetablePair> BetablePairs { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<UserWallet> UserWallets { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<AppliedBonus> AppliedBonuses { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Sport> Sports { get; set; }
        public DbSet<BetedPair> BetedPairs { get; set; }

        public BetingSystemDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AppliedBonus>()
                .HasKey(b => new {b.TicketId, b.BonusName});
        }
    }
}
