using System.Text.Json.Serialization;

namespace Checkpoint_14.Models
{
    public class Student
    {
        public int StudentId { get; set; }
        public string Name { get; set; }
        [JsonIgnore]
        public ICollection<StudentCourse>? StudentCourses { get; set; }
    }

}
