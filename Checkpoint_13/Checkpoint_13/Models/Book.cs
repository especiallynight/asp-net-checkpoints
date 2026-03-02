using System.Text.Json.Serialization;

namespace Checkpoint_13.Models
{
    public class Book
    {
        public int BookId { get; set; }
        public string Title { get; set; }
        public int AuthorId { get; set; }
        [JsonIgnore]
        public Author? Author { get; set; }
    }

}
