using System.ComponentModel.DataAnnotations.Schema;

namespace library_system.Models
{
    public class Loan
    {
        public enum enStatus
        {
            Loaned, Returned
        }
        public int loanId { get; set; }
        public string loanerName { get; set; }
        public string phoneNumber { get; set; }
        [ForeignKey("Book")] public int bookId { get; set; }
        public Book Book { get; set; }
        public DateOnly loanDate { get; set; }
        public DateOnly returnDate { get; set; }
        public enStatus status { get; set; }
    }
}