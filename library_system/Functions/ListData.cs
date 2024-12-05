using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using library_system.Models;

namespace library_system.Functions
{
    public class ListData
    {
        public static void AllBooks()
        {
            using (var context = new AppDbContext())
            {
                try
                {
                    var books = context.Books
                        .Include(b => b.Authors)
                        .ToList();

                    foreach(var b in books)
                    {
                        for (int i = 0; i < 2; i++)
                        {
                            System.Console.WriteLine(b.Authors[i].);
                        }
                        System.Console.WriteLine($"{b.bookName} {b}");
                    }
                }
                catch (Exception ex)
                {
                    Console.Clear();
                    System.Console.WriteLine($"{ex.Message} Try again.\n");
                }
            }
        }
        public static void BooksFromAuthor()
        {

        }
        public static void BookWrittenBy()
        {

        }
        public static void CurrentLoans()
        {

        }
        public static void LoanHistory()
        {

        }
    }
}