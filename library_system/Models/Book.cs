using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace library_system.Models
{
    public class Book
    {
        public int bookId { get; set; }
        public string bookName { get; set; }
        public DateOnly datePublished { get; set; }

        // TEST
        public ICollection<Author> Authors { get; set; }
        public ICollection<Loan> Loans { get; set; }
    }
}