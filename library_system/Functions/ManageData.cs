using System;
using library_system.Models;

namespace library_system.Functions
{
    public class ManageData
    {
        public static void AddBook()
        {
            System.Console.WriteLine("ADDING NEW BOOK\n");

            using (var context = new AppDbContext())
            {
                var transaction = context.Database.BeginTransaction();
                try
                {
                    // User choooses name of book, throws exception if input is invalid
                    System.Console.Write("Enter name of book: ");
                    string nameInput = Console.ReadLine();
                    if (string.IsNullOrEmpty(nameInput))
                    {
                        throw new Exception("Name input is invalid.");
                    }

                    // Checks if book is already created
                    var books = context.Books.ToList();
                    foreach (var book in books)
                    {
                        if (book.bookName == nameInput)
                        {
                            throw new Exception ("A book with this name is already registered.");
                        }
                    }

                    // User chooses date when the book was published, ParseExacct throws exception if format is invalid
                    System.Console.Write("Enter the year the book was published (yyyy-mm-dd): ");
                    DateOnly dateInput = DateOnly.ParseExact(Console.ReadLine(), "yyyy-MM-dd");

                    // Creates new object with users inputs and adds to database
                    var newBook = new Book
                    {
                        bookName = nameInput,
                        datePublished = dateInput
                    };
                    context.Books.Add(newBook);

                    // Saves changes to database and commit transaction if no exceptions where thrown
                    context.SaveChanges();
                    transaction.Commit();

                    Console.Clear();
                    System.Console.WriteLine($"{nameInput} was successfully added to the library database!\n");
                }
                catch (Exception ex)
                {
                    // Rolls back the transaction if an exception was thrown to make sure all necessary data has been added correctly
                    transaction.Rollback();
                    Console.Clear();
                    System.Console.WriteLine($"{ex.Message} Try again.\n");
                }
            }
        }
        public static void AddAuthor()
        {
            System.Console.WriteLine("ADDING NEW AUTHOR\n");

            using (var context = new AppDbContext())
            {
                var transaction = context.Database.BeginTransaction();
                try
                {
                    // User choooses name of book, throws exception if input is invalid
                    System.Console.Write("Enter name of author: ");
                    string nameInput = Console.ReadLine();
                    if (string.IsNullOrEmpty(nameInput))
                    {
                        throw new Exception("Name input is invalid. Try again.");
                    }

                    // Creates new object with users inputs and adds to database
                    var newAuthor = new Author
                    {
                        authorName = nameInput,
                    };
                    context.Authors.Add(newAuthor);

                    // Saves changes to database and commit transaction if no exceptions where thrown
                    context.SaveChanges();
                    transaction.Commit();
                    
                    Console.Clear();
                    System.Console.WriteLine($"{nameInput} was successfully added to the library database!\n");
                }
                catch (Exception ex)
                {
                    // Rolls back the transaction if an exception was thrown to make sure all necessary data has been added correctly
                    transaction.Rollback();
                    System.Console.WriteLine($"{ex.Message}. Try again.");
                }
            }
        }
        public static void BookAuthorRelation()
        {

        }
        public static void CreateLoan()
        {

        }
        public static void DeleteData()
        {

        }
        public static void AddSeedData()
        {

        }
    }
}