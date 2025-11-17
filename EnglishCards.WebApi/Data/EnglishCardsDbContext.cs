using EnglishCards.WebApi.Models;
using Microsoft.EntityFrameworkCore;

namespace EnglishCards.WebApi.Data
{
    public class EnglishCardsDbContext : DbContext
    {
        public EnglishCardsDbContext(DbContextOptions<EnglishCardsDbContext> options)
            : base(options)
        {
        }

        public DbSet<Deck> Decks { get; set; }
        public DbSet<WordCard> WordCards { get; set; }
        public DbSet<User> Users => Set<User>();
        public DbSet<TaskItem> TaskItems => Set<TaskItem>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>()
            .Property(u => u.Name).HasMaxLength(100).IsRequired();

            modelBuilder.Entity<TaskItem>()
            .Property(t => t.Title).HasMaxLength(120).IsRequired();
            modelBuilder.Entity<User>().HasData(
                new User { Id = 1, Name = "Alice", Email = "alice@example.com" },
                new User { Id = 2, Name = "Bob",   Email = "bob@example.com" }
            );
        }
    }
}
