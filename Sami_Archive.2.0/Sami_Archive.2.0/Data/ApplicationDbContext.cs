using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Sami_Archive._2._0.Models;

namespace Sami_Archive._2._0.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Author> Authors { get; set; } = null!;
        public DbSet<Book> Books { get; set; } = null!;
        public DbSet<Genre> Genres { get; set; } = null!;
        public DbSet<List> Lists { get; set; } = null!;

        public DbSet<BookAuthor> BookAuthors { get; set; } = null!;
        public DbSet<BookGenre> BookGenres { get; set; } = null!;

        public DbSet<ListUser> ListUsers { get; set; } = null!;
        public DbSet<ListingBook> listingBooks { get; set; } = null!;
        public DbSet<UserBookComment> UserBookComments { get; set; } = null!;
        public DbSet<UserBookRating> UserBookRating { get; set; } = null!;

        // Customise columns and mapping

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Define the composite key across some IDS

            builder.Entity<Book>()
                .HasMany(a => a.Authors)
                .WithMany(b => b.Books)
                .UsingEntity<BookAuthor>(
                    j => j.HasOne(ba => ba.Author).WithMany().HasForeignKey(ba => ba.AuthorID),
                    j => j.HasOne(ba => ba.Book).WithMany().HasForeignKey(ba => ba.BookID),
                    j => { j.HasKey(t => new { t.BookID, t.AuthorID }); }
        );

            builder.Entity<Book>()
                .HasMany(g => g.Genres)
                .WithMany(b => b.Books)
                .UsingEntity<BookGenre>(
                    j => j.HasOne(bg => bg.Genre).WithMany().HasForeignKey(bg => bg.GenreID),
                    j => j.HasOne(bg => bg.Book).WithMany().HasForeignKey(bg => bg.BookID),
                    j => { j.HasKey(t => new { t.BookID, t.GenreID }); }
        );

            // Adding rating, book -> userbookrating <- user

            builder.Entity<UserBookRating>(ubr =>
            {
                // Making composite key
                ubr.HasKey(x => new { x.UserId, x.BookId }); 

                ubr.HasOne(x => x.User)
                    .WithMany(ubr => ubr.UserBookRatings)
                    .HasForeignKey(ubr => ubr.UserId)
                    .OnDelete(DeleteBehavior.Cascade);

                ubr.HasOne(x => x.Book)
                    .WithMany(ubr => ubr.Ratings)
                    .HasForeignKey(ubr => ubr.BookId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Adding comment, book -> userbookcomment <- user

            builder.Entity<UserBookComment>(ubc =>
            {
                // Making composite key
                ubc.HasKey(x => new { x.UserId, x.BookId });

                ubc.HasOne(x => x.User)
                    .WithMany(ubc => ubc.UserBookComments)
                    .HasForeignKey(ubc => ubc.UserId)
                    .OnDelete(DeleteBehavior.Cascade);


                ubc.HasOne(x => x.Book)
                    .WithMany(ubc => ubc.Comments)
                    .HasForeignKey(ubc => ubc.BookId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Adding list, list -> list user <- user

            builder.Entity<ListUser>(lu =>
            {
                // Making Composite key
                lu.HasKey(x => new { x.ListId, x.UserId });

                lu.HasOne(x => x.List)
                    .WithMany(x => x.ListUser)
                    .HasForeignKey(lu => lu.ListId)
                    .OnDelete(DeleteBehavior.Cascade);

                lu.HasOne(x => x.User)
                    .WithMany(x => x.ListUsers)
                    .HasForeignKey(x => x.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Adding list, list -> listing book <- book

            builder.Entity<ListingBook>(lb =>
            {
                // Making Composite Key
                lb.HasKey(x => new { x.ListId, x.BookId });

                lb.HasOne(x => x.List)
                    .WithMany(x => x.ListingBooks)
                    .HasForeignKey(x => x.ListId)
                    .OnDelete(DeleteBehavior.Cascade);

                lb.HasOne(x => x.Book)
                    .WithMany(x => x.ListingBooks)
                    .HasForeignKey(x => x.BookId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}