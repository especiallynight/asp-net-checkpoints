using System.ComponentModel.DataAnnotations;

namespace Checkpoint_9.Models
{
    public class CreateUserViewModel
    {
        [Required] public string FirstName { get; set; }
        [Required] public string LastName { get; set; }
        [Required, EmailAddress] public string Email { get; set; }
        [Required, DataType(DataType.Password)] public string Password { get; set; }
    }
}
