using System;
using library_system.Models;
using Microsoft.EntityFrameworkCore;

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

                    // Checks if book is already created, if so, an exception is thrown
                    var books = context.Books.ToList();
                    foreach (var book in books)
                    {
                        if (book.bookName == nameInput)
                        {
                            throw new Exception("A book with this name is already registered.");
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
                    // User choooses name of author, throws exception if input is invalid
                    System.Console.Write("Enter name of author: ");
                    string nameInput = Console.ReadLine();
                    if (string.IsNullOrEmpty(nameInput))
                    {
                        throw new Exception("Name input is invalid. Try again.");
                    }

                    // Checks if author is already created, if so, an exception is thrown
                    var authors = context.Authors.ToList();
                    foreach (var author in authors)
                    {
                        if (author.authorName == nameInput)
                        {
                            throw new Exception("An author with this name is already registered.");
                        }
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
                    Console.Clear();
                    System.Console.WriteLine($"{ex.Message} Try again.\n");
                }
            }
        }
        public static void BookAuthorRelation()
        {
            System.Console.WriteLine("ASSIGNING AUTHOR TO BOOK");

            using (var context = new AppDbContext())
            {
                var transaction = context.Database.BeginTransaction();
                try
                {
                    // Declares unassigned objects to later be assigned by user
                    Book chosenBook = null;
                    Author chosenAuthor = null;

                    // Asks user to input the name of the book they want to add an author too
                    // Throws exception if input is invalid in invalid format
                    System.Console.Write("Enter name of book: ");
                    string bookNameInput = Console.ReadLine();
                    if (string.IsNullOrEmpty(bookNameInput))
                    {
                        throw new Exception("Name input is invalid. Try again.");
                    }

                    // Checks if book exists in database, if so, entity is assigned to the object
                    // If not, an exception is thrown
                    var books = context.Books.ToList();
                    foreach (var book in books)
                    {
                        if (book.bookName == bookNameInput)
                        {
                            chosenBook = book;
                            break;
                        }
                    }
                    if (chosenBook == null)
                    {
                        throw new Exception("A book with this name doesn't exist in the database.");
                    }

                    System.Console.Write("Enter name of author: ");
                    string authorNameInput = Console.ReadLine();
                    if (string.IsNullOrEmpty(authorNameInput))
                    {
                        throw new Exception("Name input is invalid. Try again.");
                    }

                    // Checks if author exists in database, if so, entity is assigned to the object
                    // If not, an exception is thrown
                    var authors = context.Authors.ToList();
                    foreach (var author in authors)
                    {
                        if (author.authorName == authorNameInput)
                        {
                            chosenAuthor = author;
                            break;
                        }
                    }
                    if (chosenAuthor == null)
                    {
                        throw new Exception("An author with this name doesn't exist in the database.");
                    }

                    // Creates new object with users inputs and adds to database
                    var newBookAuthorRelation = new BookAuthor
                    {
                        bookId = chosenBook.bookId,
                        authorId = chosenAuthor.authorId
                    };
                    context.BookAuthor.Add(newBookAuthorRelation);

                    // Saves changes to database and commit transaction if no exceptions where thrown
                    context.SaveChanges();
                    transaction.Commit();

                    Console.Clear();
                    System.Console.WriteLine($"{authorNameInput} was successfully added as an author to the book {bookNameInput}!\n");
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
        public static void CreateLoan()
        {
            using (var context = new AppDbContext())
            {
                var transaction = context.Database.BeginTransaction();
                try
                {
                    // Declares unassigned object to later be assigned by user
                    Book chosenBook = null;

                    // Asks user to input the name of the book they want to add an loan
                    // Throws exception if input is invalid in invalid format
                    System.Console.Write("Enter name of book you want to loan: ");
                    string bookNameInput = Console.ReadLine();
                    if (string.IsNullOrEmpty(bookNameInput))
                    {
                        throw new Exception("Name input is invalid.");
                    }

                    // Checks if book exists in database, if so, entity is assigned to the object
                    // If not, an exception is thrown
                    var books = context.Books
                        .Include(b => b.Loans)
                        .ToList();
                    foreach (var book in books)
                    {
                        if (book.bookName == bookNameInput)
                        {
                            // Checks if book is available to be loaned, otherwise throws an exception
                            foreach (var l in book.Loans)
                            {
                                if (l.status == Loan.enStatus.Loaned)
                                {
                                    throw new Exception($"This book is already loaned right now. You can loan it after {l.returnDate}.");
                                }
                            }
                            chosenBook = book;
                            break;
                        }
                        else
                        {
                            throw new Exception("A book with this name doesn't exist in the database.");
                        }
                    }

                    // Asks for the loaners name and then phone number
                    System.Console.Write("Enter name of loaner: ");
                    string loanerNameInput = Console.ReadLine();
                    if (string.IsNullOrEmpty(loanerNameInput))
                    {
                        throw new Exception("Name input is invalid.");
                    }
                    System.Console.Write("Enter phone number of loaner: ");
                    string loanerPnInput = Console.ReadLine();
                    if (string.IsNullOrEmpty(loanerPnInput))
                    {
                        throw new Exception("Phone number input is invalid.");
                    }

                    // Creating loan and saving in database, can be loaned in 30 days
                    var newLoan = new Loan
                    {
                        loanerName = loanerNameInput,
                        phoneNumber = loanerPnInput,
                        Book = chosenBook,
                        loanDate = DateOnly.ParseExact(DateTime.Now.ToString(), "yyyy-MMdd"),
                        returnDate = DateOnly.ParseExact(DateTime.Now.AddDays(30).ToString(), "yyyy-MMdd"),
                        status = Loan.enStatus.Loaned
                    };
                    context.Loans.Add(newLoan);

                    // Saves loan in database if no exception was thrown
                    context.SaveChanges();
                    transaction.Commit();

                    Console.Clear();
                    System.Console.WriteLine($"{loanerNameInput} now has a loan on {bookNameInput}. Last day to return book: {newLoan.returnDate}.");
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
        public static void DeleteData()
        {
            // Asks user what type of entity it wants to delete
            System.Console.WriteLine("What do you want to delete?");
            System.Console.WriteLine("1 - Book");
            System.Console.WriteLine("2 - Author");
            System.Console.WriteLine("3 - Loan");
            string choiceInput = Console.ReadLine();

            switch (choiceInput)
            {
                case "1":
                    DeleteBook();
                    break;
                case "2":
                    DeleteAuthor();
                    break;
                case "3":
                    DeleteLoan();
                    break;
                default:
                    Console.Clear();
                    System.Console.WriteLine("Invalid input, try again.\n");
                    break;

            }
            static void DeleteBook()
            {
                using (var context = new AppDbContext())
                {
                    var transaction = context.Database.BeginTransaction();
                    try
                    {
                        // Declares unassigned objects to later be assigned by user
                        Book chosenBook = null;

                        // Asks user to input the name of the book they want to add an author too
                        // Throws exception if input is invalid in invalid format
                        System.Console.Write("Enter name of book: ");
                        string bookNameInput = Console.ReadLine();
                        if (string.IsNullOrEmpty(bookNameInput))
                        {
                            throw new Exception("Name input is invalid.");
                        }

                        // Checks if book exists in database, if so, entity is assigned to the object
                        // If not, an exception is thrown
                        var books = context.Books.ToList();
                        foreach (var book in books)
                        {
                            if (book.bookName == bookNameInput)
                            {
                                chosenBook = book;
                                break;
                            }
                        }
                        if (chosenBook == null)
                        {
                            throw new Exception("A book with this name doesn't exist in the database.");
                        }

                        context.Books.Remove(chosenBook);

                        // Saves changes to database and commit transaction if no exceptions where thrown
                        context.SaveChanges();
                        transaction.Commit();

                        Console.Clear();
                        System.Console.WriteLine($"{chosenBook.bookName} was successfully deleted from database!\n");
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
            static void DeleteAuthor()
            {
                using (var context = new AppDbContext())
                {
                    var transaction = context.Database.BeginTransaction();
                    try
                    {
                        // Declares unassigned objects to later be assigned by user
                        Author chosenAuthor = null;

                        // Asks user to input the name of the Author they want to add an author too
                        // Throws exception if input is invalid in invalid format
                        System.Console.Write("Enter name of author: ");
                        string authorNameInput = Console.ReadLine();
                        if (string.IsNullOrEmpty(authorNameInput))
                        {
                            throw new Exception("Name input is invalid.");
                        }

                        // Checks if author exists in database, if so, entity is assigned to the object
                        // If not, an exception is thrown
                        var authors = context.Authors.ToList();
                        foreach (var author in authors)
                        {
                            if (author.authorName == authorNameInput)
                            {
                                chosenAuthor = author;
                                break;
                            }
                        }
                        if (chosenAuthor == null)
                        {
                            throw new Exception("An author with this name doesn't exist in the database.");
                        }

                        context.Authors.Remove(chosenAuthor);

                        // Saves changes to database and commit transaction if no exceptions where thrown
                        context.SaveChanges();
                        transaction.Commit();

                        Console.Clear();
                        System.Console.WriteLine($"{chosenAuthor.authorName} was successfully deleted from database!\n");
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
            static void DeleteLoan() // FORTSÄTT HÄR! Ska man använda loanID istället för att välja? Kan presentera alla lån isf
            {
                // Ask user if deleting loan is necessary
                System.Console.WriteLine("Are you sure you want to DELETE a loan? Deleting a loan will affect loan history.");
                System.Console.WriteLine("If you wish to return a loaned book, choose [Rerturn book] in MANAGE DATA menu instead.");
                System.Console.Write("Type (yes) to proceed. Type anything to go cancel.");

                string proceed = Console.ReadLine();
                if (proceed.ToLower() == "yes")
                {
                    using (var context = new AppDbContext())
                    {
                        var transaction = context.Database.BeginTransaction();
                        try
                        {
                            // Declares unassigned objects to later be assigned by user
                            Loan chosenLoan = null;

                            // Asks user to input the name of the Author they want to add an author too
                            // Throws exception if input is invalid in invalid format
                            System.Console.Write("Enter name of loaner: ");
                            string loanerNameInput = Console.ReadLine();
                            if (string.IsNullOrEmpty(loanerNameInput))
                            {
                                throw new Exception("Name input is invalid.");
                            }

                            // Asks user to input the name of the Author they want to add an author too
                            // Throws exception if input is invalid in invalid format
                            System.Console.Write("Enter name of the book that is loaned: ");
                            string loanerBookInput = Console.ReadLine();
                            if (string.IsNullOrEmpty(loanerBookInput))
                            {
                                throw new Exception("Name input is invalid.");
                            }

                            // Checks if author exists in database, if so, entity is assigned to the object
                            // If not, an exception is thrown
                            var loans = context.Loans.ToList();
                            foreach (var loan in loans)
                            {
                                if (loan.loanerName == loanerNameInput && loan.Book.bookName == loanerBookInput)
                                {
                                    chosenLoan = loan;
                                    break;
                                }
                            }
                            if (chosenLoan == null)
                            {
                                throw new Exception("This loan doesn't exist in the database.");
                            }

                            context.Loans.Remove(chosenLoan);

                            // Saves changes to database and commit transaction if no exceptions where thrown
                            context.SaveChanges();
                            transaction.Commit();

                            Console.Clear();
                            System.Console.WriteLine($"Loan on {chosenLoan} by {chosenLoan.loanerName} was successfully deleted from database!\n");
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
                else
                {
                    return;
                }
            }

        }
        public static void AddSeedData()
        {

        }
    }
}