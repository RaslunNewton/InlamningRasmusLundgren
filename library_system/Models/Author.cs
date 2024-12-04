using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace library_system.Models
{
    public class Author
    {
        public int authorId { get; set; }
        public string authorName { get; set; }
        public ICollection<Book> Books { get; set; }
    }
}