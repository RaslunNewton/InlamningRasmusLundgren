using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using library_system.Models;
using System.IO.Compression;
using System.Reflection.Metadata;

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
                        System.Console.WriteLine($"{b.bookName,-29}{b.datePublished.ToString("yyyy-MM-dd"),-34}{sAuthors,-30}");
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
                    Author chosenAuthor = null;

                    // User choooses name of author, throws exception if input is invalid
                    System.Console.Write("Enter name of author: ");
                    string nameInput = Console.ReadLine();
                    if (string.IsNullOrWhiteSpace(nameInput))
                    {
                        throw new Exception("Name input is invalid. Try again.");
                    }

                    var authors = context.Authors
                        .Include(a => a.Books)
                        .ToList();

                    foreach (var a in authors)
                    {
                        if (a.authorName == nameInput)
                        {
                            chosenAuthor = a;
                        }
                    }
                    if (chosenAuthor == null)
                    {
                        throw new Exception("Author doesn't exist in database.");
                    }

                    Console.Clear();

                    System.Console.WriteLine($"Books written by {chosenAuthor.authorName}:\n");
                    foreach (var b in chosenAuthor.Books)
                    {
                        System.Console.WriteLine($"{b.bookName}");
                    }

                    if (!chosenAuthor.Books.Any())
                    {
                        throw new Exception("This author doesn't have any registered books as of now.");
                    }
                    System.Console.WriteLine($"\nPress any key to continue.");
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
        public static void BookWrittenBy()
        {
            using (var context = new AppDbContext())
            {
                try
                {
                    Book chosenBook = null;

                    // User choooses name of book, throws exception if input is invalid
                    System.Console.Write("Enter name of book: ");
                    string nameInput = Console.ReadLine();
                    if (string.IsNullOrWhiteSpace(nameInput))
                    {
                        throw new Exception("Name input is invalid. Try again.");
                    }

                    var books = context.Books
                        .Include(b => b.Authors)
                        .ToList();

                    foreach (var b in books)
                    {
                        if (b.bookName == nameInput)
                        {
                            chosenBook = b;
                        }
                    }
                    if (chosenBook == null)
                    {
                        throw new Exception("Book doesn't exist in database.");
                    }

                    Console.Clear();

                    System.Console.WriteLine($"Contributing authors to {chosenBook.bookName}:\n");
                    foreach (var a in chosenBook.Authors)
                    {
                        System.Console.WriteLine($"{a.authorName}");
                    }

                    if (!chosenBook.Authors.Any())
                    {
                        throw new Exception("Book doesn't have any assigned authors.");
                    }
                    System.Console.WriteLine($"\nPress any key to continue.");
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
        public static void CurrentLoans()
        {
            using (var context = new AppDbContext())
            {
                try
                {
                    // Creates list of book entities including their authors
                    var books = context.Books
                        .Include(b => b.Loans)
                        .ToList();

                    // Iterates through every book in "books" and adds every author of each book to an string which is
                    // represented along with each book
                    System.Console.WriteLine($"{"BOOK NAME",-29}{"LOAN ID",-17}{"LOANER",-26}{"STATUS",-16}{"RETURN DATE",-15}");
                    System.Console.WriteLine("----------------------------------------------------------------------------------------------------");

                    if (!books.Any(b => b.Loans.Any(l => l.status == Loan.enStatus.Loaned)))
                    {
                        throw new Exception("No active loans as of now.");
                    }
                    foreach (var b in books)
                    {
                        foreach (var l in b.Loans)
                        {
                            if (l.status == Loan.enStatus.Loaned)
                            {
                                System.Console.WriteLine($"{b.bookName,-29}{l.loanId,-17}{l.loanerName,-26}{l.status,-16}{l.returnDate.ToString("yyyy-MM-dd"),-15}");
                            }
                        }
                    }
                    Console.WriteLine();
                }
                catch (Exception ex)
                {
                    Console.Clear();
                    System.Console.WriteLine($"{ex.Message} Try again.\n");
                }
            }
        }
        public static void LoanHistory()
        {
            using (var context = new AppDbContext())
            {
                try
                {
                    // Creates list of book entities including their authors
                    var books = context.Books
                        .Include(b => b.Loans)
                        .ToList();

                    if (!books.Any(b => b.Loans.Any()))
                    {
                        throw new Exception("Loan history is empty as of now.");
                    }

                    System.Console.WriteLine($"{"BOOK NAME",-29}{"LOAN ID",-17}{"LOANER",-26}{"STATUS",-16}{"RETURN DATE",-15}");
                    System.Console.WriteLine("----------------------------------------------------------------------------------------------------");
                    foreach (var b in books)
                    {
                        foreach (var l in b.Loans)
                        {
                            System.Console.WriteLine($"{b.bookName,-29}{l.loanId,-17}{l.loanerName,-26}{l.status,-16}{l.returnDate.ToString("yyyy-MM-dd"),-15}");
                        }
                    }
                    Console.WriteLine();
                }
                catch (Exception ex)
                {
                    Console.Clear();
                    System.Console.WriteLine($"{ex.Message} Try again.\n");
                }
            }
        }
    }
}