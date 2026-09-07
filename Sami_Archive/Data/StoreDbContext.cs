using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Sami_Archive.Models;
using System.Reflection.Emit;

namespace Sami_Archive.Data
{
    public class StoreDbContext : IdentityDbContext<ApplicationUser>
    {
        public StoreDbContext(DbContextOptions<StoreDbContext> options) : base(options) { }

        public DbSet<Book> Books { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<BookAuthor> BookAuthor { get; set; }
        public DbSet<BookGenre> BookGenre { get; set; }

        // This is needed if I want to customise a Identiy's table/column mapping
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Defining the composite Primary key across all three IDs

            modelBuilder.Entity<Book>()
                .HasMany(b => b.Authors)
                .WithMany(a => a.Books)
                .UsingEntity<BookAuthor>(
                    j => j.HasOne(ba => ba.Author).WithMany().HasForeignKey(ba => ba.AuthorID),
                    j => j.HasOne(ba => ba.Book).WithMany().HasForeignKey(ba => ba.BookID),
                    j => { j.HasKey(t => new { t.BookID, t.AuthorID }); }
                );

            modelBuilder.Entity<Book>()
                .HasMany(b => b.Genres)
                .WithMany(g => g.Books)
                .UsingEntity<BookGenre>(
                    j => j.HasOne(bg => bg.Genre).WithMany().HasForeignKey(bg => bg.GenreID),
                    j => j.HasOne(bg => bg.Book).WithMany().HasForeignKey(bg => bg.BookID),
                    j => { j.HasKey(t => new { t.BookID, t.GenreID }); }
                );
        }
    }
}
