using System.ComponentModel.DataAnnotations.Schema;

namespace library_system.Models
{
    public class Book
    {
        public int bookId { get; set; }
        public string bookName { get; set; }
        public DateTime yearPublished { get; set; }
    }
}