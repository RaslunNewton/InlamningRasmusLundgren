using System.ComponentModel.DataAnnotations.Schema;

namespace library_system.Models
{
    public class Author
    {
        public int authorId { get; set; }
        public string authorName { get; set; }
    }
}