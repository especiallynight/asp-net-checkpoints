using System.Text.Json.Serialization;

namespace Checkpoint_14.Models
{
    public class Course
    {
        public int CourseId { get; set; }
        public string Title { get; set; }
        public int? TeacherId { get; set; }

        [JsonIgnore]
        public Teacher? Teacher { get; set; }
        [JsonIgnore]

        public ICollection<StudentCourse>? StudentCourses { get; set; }
    }

}
