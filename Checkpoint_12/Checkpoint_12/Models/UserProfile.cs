using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Checkpoint_12.Models
{
    public class UserProfile
    {
        public int UserProfileId { get; set; }
        public string Bio { get; set; }
        public int UserId { get; set; }
        [JsonIgnore]
        public User? User { get; set; }

    }
}
