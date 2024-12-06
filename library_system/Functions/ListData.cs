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
                    // Creates list of book entities including their authors
                    var books = context.Books
                        .Include(b => b.Authors)
                        .ToList();

                    // Iterates through every book in "books" and adds every author of each book to an string which is
                    // represented along with each book
                    System.Console.WriteLine($"{"BOOK NAME",-29}{"DATE PUBLISHED",-34}{"AUTHOR(s)",-30}");
                    System.Console.WriteLine("---------------------------------------------------------------------------------------------");
                    foreach (var b in books)
                    {
                        string sAuthors = null;
                        if (!b.Authors.Any())
                        {
                            sAuthors += "NO ASSIGNED AUTHOR\n";
                        }
                        foreach (var a in b.Authors)
                        {
                            sAuthors += $"{a.authorName}\n{"",-63}";
                        }
                        System.Console.WriteLine($"{b.bookName,-29}{b.datePublished,-34}{sAuthors,-30}");
                    }
                    System.Console.WriteLine($"Press any key to continue.");
                    Console.ReadKey();
                    Console.Clear();
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
            using (var context = new AppDbContext())
            {
                try
                {
                    // User choooses name of author, throws exception if input is invalid
                    System.Console.Write("Enter name of author: ");
                    string nameInput = Console.ReadLine();
                    if (string.IsNullOrEmpty(nameInput))
                    {
                        throw new Exception("Name input is invalid. Try again.");
                    }

                    var books = context.Books
                        .Include(b => b.Authors)
                        .ToList();

                    foreach (var b in books)
                    {
                        foreach (var a in b.Authors)
                        {
                            if(!a.Books.Any())
                            {
                                throw new Exception("This author haven't been assigned to any books in this database.");
                            }
                            if (a.authorName == nameInput)
                            {
                                System.Console.WriteLine($"{b.bookName,-29}{a.authorName,-17}");
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    System.Console.WriteLine(ex.Message);
                }
            }
        }
        public static void BookWrittenBy()
        {

        }
        public static void CurrentLoans()
        {

        }
        public static void LoanHistory()
        {
            using (var context = new AppDbContext())
            {
                // Creates list of book entities including their authors
                var books = context.Books
                    .Include(b => b.Loans)
                    .ToList();

                // Iterates through every book in "books" and adds every author of each book to an string which is
                // represented along with each book
                System.Console.WriteLine($"{"BOOK NAME",-29}{"LOAN ID",-17}{"LOANER",-26}{"STATUS",-26}");
                System.Console.WriteLine("---------------------------------------------------------------------------------------------");
                foreach (var b in books)
                {
                    foreach (var l in b.Loans)
                    {
                        System.Console.WriteLine($"{b.bookName,-29}{l.loanId,-17}{l.loanerName,-26}{l.status,-26}");
                    }
                }
                Console.WriteLine();
            }
        }
    }
}