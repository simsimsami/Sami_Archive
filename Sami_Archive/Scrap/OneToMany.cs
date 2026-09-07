using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sami_Archive.Scrap
{
    public class OneToMany
    {
        // Junction class
        public class Student { public int Id { get; set; } }
        public class Course { public int Id { get; set; } }
        public class Semester { public int Id { get; set; } }

        public class Enrollment
        {
            // Composite key properties / Foreign Keys
            public int StudentId { get; set; }
            public int CourseId { get; set; }
            public int SemesterId { get; set; }

            // Navigation properties
            public Student Student { get; set; } = null!;
            public Course Course { get; set; } = null!;
            public Semester Semester { get; set; } = null!;
        }
    }
}
