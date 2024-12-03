using System.ComponentModel.DataAnnotations.Schema;

namespace library_system.Models
{
    public class BookAuthor     // Bridge table
    {
        public int BookAuthorId { get; set; }
        [ForeignKey("Book")] public int bookId { get; set; }
        public Book Book { get; set; }
        [ForeignKey("Author")] public int authorId { get; set; }
        public Author Author { get; set; }
    }
}