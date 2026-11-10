using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sami_Archive.Scrap
{
    public class ManyToMany
    {
        public class Student
        {
            public int Id { get; set; }
            public string Name { get; set; }
            ICollection<Course> Courses { get; set; } = new List<Course>();
        }

        public class Course
        {
            public int Id { get; set; }
            public string Title { get; set; }

            ICollection<Student> Students { get; set; } = new List<Student>();  
        }
    }
}
