//using Microsoft.AspNetCore.Identity;
//using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
//using Microsoft.EntityFrameworkCore;
//using Sami_Archive.Data;
//using Sami_Archive.Models;

//namespace Sami_Archive.Scrap
//{
//    public class DbContext
//    {
//        // 1. Inherit from DbContext (or IdentityDbcontext if mapping users
//        public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
//        {
//            // 2. Add constructor for dependency injection
//            public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
//            {
//            }

//            // 3. Expose the tables as DbSets
//            public DbSet<OneToMany.Student> Students { get; set; }
//            public DbSet<OneToMany.Course> Courses { get; set; }
//            public DbSet<OneToMany.Semester> Semesters { get; set; }
//            public DbSet<OneToMany.Enrollment> Enrollments { get; set; }

//            protected override void OnModelCreating(ModelBuilder modelBuilder)
//            {

//                base.OnModelCreating(modelBuilder);
                
//                // Define the composite primary key across all three IDs
//                modelBuilder.Entity<OneToMany.Enrollment>()
//                    .HasKey(e => new { e.StudentId, e.CourseId, e.SemesterId });

//                // Establish the one -to-many relationships
//                modelBuilder.Entity<OneToMany.Enrollment>()
//                    .HasOne(e => e.Student)
//                    .WithMany() // Or .WithMany(s => s.Enrollments) if you add a collection to student
//                    .HasForeignKey(e => e.StudentId);

//                modelBuilder.Entity<OneToMany.Enrollment>()
//                    .HasOne(e => e.Course)
//                    .WithMany() // Or .WithMany(c => c.Enrollments) if you add a collection to course
//                    .HasForeignKey(e => e.CourseId);

//                modelBuilder.Entity<OneToMany.Enrollment>()
//                    .HasOne(e => e.Semester)
//                    .WithMany() // Or .WithMany(s => s.Enrollments) if you add a collection to semester
//                    .HasForeignKey(e => e.SemesterId);
//            }
//        }
//    }
//}
