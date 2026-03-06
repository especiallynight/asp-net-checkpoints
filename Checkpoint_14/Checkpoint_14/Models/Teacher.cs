using System.Text.Json.Serialization;

namespace Checkpoint_14.Models
{
    public class Teacher
    {
        public int TeacherId { get; set; }
        public string Name { get; set; }
        [JsonIgnore]
        public ICollection<Course>? Courses { get; set; } 
    }
}
