namespace Checkpoint_12.Models
{
    public class User
    {
        public int UserId { get; set; }
        public string Name { get; set; }

        public UserProfile? Profile { get; set; }

    }
}
