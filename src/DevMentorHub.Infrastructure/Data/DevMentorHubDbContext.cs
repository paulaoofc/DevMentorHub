using DevMentorHub.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DevMentorHub.Infrastructure.Data
{
    public class DevMentorHubDbContext : DbContext
    {
        public DevMentorHubDbContext(DbContextOptions<DevMentorHubDbContext> options) : base(options) { }

        public DbSet<User> Users => Set<User>();
        public DbSet<Project> Projects => Set<Project>();
        public DbSet<Snippet> Snippets => Set<Snippet>();
        public DbSet<CodeReview> CodeReviews => Set<CodeReview>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(b =>
            {
                b.HasKey(x => x.Id);
                b.Property(x => x.Email).IsRequired();
                b.Property(x => x.PasswordHash).IsRequired();
                b.Property(x => x.Name).HasMaxLength(200);
            });

            modelBuilder.Entity<Project>(b =>
            {
                b.HasKey(x => x.Id);
                b.Property(x => x.Title).IsRequired().HasMaxLength(200);
                b.HasOne(x => x.Owner)
                 .WithMany()
                 .HasForeignKey(x => x.OwnerId)
                 .OnDelete(DeleteBehavior.Restrict);
                b.HasMany(x => x.Snippets)
                 .WithOne(s => s.Project)
                 .HasForeignKey(s => s.ProjectId);
            });

            modelBuilder.Entity<Snippet>(b =>
            {
                b.HasKey(x => x.Id);
                b.Property(x => x.Language).IsRequired().HasMaxLength(100);
                b.Property(x => x.Code).IsRequired();
                b.HasOne(x => x.Owner)
                 .WithMany()
                 .HasForeignKey(x => x.OwnerId)
                 .OnDelete(DeleteBehavior.Restrict);
                b.HasMany(x => x.CodeReviews)
                 .WithOne(r => r.Snippet)
                 .HasForeignKey(r => r.SnippetId);
            });

            modelBuilder.Entity<CodeReview>(b =>
            {
                b.HasKey(x => x.Id);
                b.Property(x => x.ResponseText).HasMaxLength(100_000);
            });
        }
    }
}
