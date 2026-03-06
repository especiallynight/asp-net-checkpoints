using System.Text.Json.Serialization;

namespace Checkpoint_14.Models
{
    public class StudentCourse
    {
        public int StudentId { get; set; }
        [JsonIgnore]
        public Student? Student { get; set; }
        public int CourseId { get; set; }
        [JsonIgnore]
        public Course? Course { get; set; }
    }


}
