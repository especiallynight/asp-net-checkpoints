using System.ComponentModel.DataAnnotations;

namespace Checkpoint_9.Models
{
    public class EditUserViewModel
    {
        public string Id { get; set; } = string.Empty; 

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
    }
}