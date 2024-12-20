using System;
using System.Transactions;
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
                    if (string.IsNullOrWhiteSpace(nameInput))
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
                    if (string.IsNullOrWhiteSpace(nameInput))
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

                    // Asks user to input the name of the book they want to add an author to
                    // Throws exception if input is invalid in invalid format
                    System.Console.Write("Enter name of book: ");
                    string bookNameInput = Console.ReadLine();
                    if (string.IsNullOrWhiteSpace(bookNameInput))
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
                    if (string.IsNullOrWhiteSpace(authorNameInput))
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
                    if (string.IsNullOrWhiteSpace(bookNameInput))
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
                                    throw new Exception($"This book is already loaned right now. You can loan it after {l.returnDate.ToString("yyyy-MM-dd")}.");
                                }
                            }
                            chosenBook = book;
                            break;
                        }
                    }
                    if (chosenBook == null)
                    {
                        throw new Exception("A book with this name doesn't exist in the database.");
                    }

                    // Asks for the loaners name and then phone number
                    System.Console.Write("Enter name of loaner: ");
                    string loanerNameInput = Console.ReadLine();
                    if (string.IsNullOrWhiteSpace(loanerNameInput))
                    {
                        throw new Exception("Name input is invalid.");
                    }
                    System.Console.Write("Enter phone number of loaner: ");
                    string loanerPnInput = Console.ReadLine();
                    if (string.IsNullOrWhiteSpace(loanerPnInput))
                    {
                        throw new Exception("Phone number input is invalid.");
                    }

                    // Creating loan and saving in database, can be loaned in 30 days
                    var newLoan = new Loan
                    {
                        loanerName = loanerNameInput,
                        phoneNumber = loanerPnInput,
                        Book = chosenBook,
                        loanDate = DateOnly.ParseExact(DateTime.Now.ToString("yyyy-MMdd"), "yyyy-MMdd"),
                        returnDate = DateOnly.ParseExact(DateTime.Now.AddDays(30).ToString("yyyy-MMdd"), "yyyy-MMdd"),
                        status = Loan.enStatus.Loaned
                    };
                    context.Loans.Add(newLoan);

                    // Saves loan in database if no exception was thrown
                    context.SaveChanges();
                    transaction.Commit();

                    Console.Clear();
                    System.Console.WriteLine($"{loanerNameInput} now has a loan on {bookNameInput}. Last day to return book: {newLoan.returnDate.ToString("yyyy-MM-dd")}.\n");
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
        public static void ReturnLoan()
        {
            using (var context = new AppDbContext())
            {
                var transaction = context.Database.BeginTransaction();
                {
                    try
                    {
                        // Declares unassigned objects to later be assigned by user
                        Loan chosenLoan = null;

                        // Asks user to enter their phone number for verification
                        System.Console.Write("Enter phone number to see your loans: ");
                        string pnInput = Console.ReadLine();
                        if (string.IsNullOrWhiteSpace(pnInput))
                        {
                            throw new Exception("Phone number input is invalid.");
                        }

                        // Makes list of loans that are currently loaned
                        var currentLoans = context.Loans
                            .Where(l => l.status == Loan.enStatus.Loaned)
                            .Where(l => l.phoneNumber == pnInput)
                            .Include(l => l.Book)
                            .ToList();

                        // Throws exception if currentLoans is empty
                        if (!currentLoans.Any())
                        {
                            throw new Exception("You have no active loans.");
                        }

                        Console.Clear();
                        System.Console.WriteLine($"{"BOOK NAME",-29}{"LOAN ID",-17}{"LOANER",-26}{"STATUS",-26}");
                        System.Console.WriteLine("---------------------------------------------------------------------------------");
                        foreach (var l in currentLoans)
                        {
                            if (l.phoneNumber == pnInput)
                            {
                                System.Console.WriteLine($"{l.Book.bookName,-29}{l.loanId,-17}{l.loanerName,-26}{l.status,-26}");
                            }
                        }

                        // Asks user to input the ID of the loan they want to delete
                        // Throws exception if input unable to be parsed as int
                        System.Console.WriteLine("\nEnter LOAN ID of the loan you want to return");
                        string idInput = Console.ReadLine();
                        int.TryParse(idInput, out int chosenId);

                        foreach (var l in currentLoans)
                        {
                            if (l.loanId == chosenId)
                            {
                                chosenLoan = l;
                            }
                        }
                        if (chosenLoan == null)
                        {
                            throw new Exception("This loan doesn't exist in the database.");
                        }

                        // If no exceptions are thrown, status is changed to returned
                        chosenLoan.status = Loan.enStatus.Returned;

                        context.SaveChanges();
                        transaction.Commit();

                        Console.Clear();
                        System.Console.WriteLine($"{chosenLoan.Book.bookName} was successfully returned!\n");
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
        }
        public static void DeleteData()
        {
            // Asks user what type of entity it wants to delete
            System.Console.WriteLine("What do you want to delete?");
            System.Console.WriteLine("1 - Book");
            System.Console.WriteLine("2 - Author");
            System.Console.WriteLine("3 - Loan");
            System.Console.WriteLine("4 - Clear database");
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
                case "4":
                    DeleteAll();
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

                        System.Console.WriteLine("WARNING: Deleting a book will also delete all related loan history.");

                        // Asks user to input the name of the book they want to add an author too
                        // Throws exception if input is invalid in invalid format
                        System.Console.Write("Enter name of book: ");
                        string bookNameInput = Console.ReadLine();
                        if (string.IsNullOrWhiteSpace(bookNameInput))
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
                        if (string.IsNullOrWhiteSpace(authorNameInput))
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
            static void DeleteLoan()
            {
                // DISCLAIMER: Ask user if deleting loan is necessary
                Console.Clear();
                System.Console.WriteLine("Are you sure you want to DELETE a loan? Deleting a loan will effect loan history.\n");
                System.Console.WriteLine("If you wish to return a loaned book, choose [Rerturn book] in MANAGE DATA menu instead.\n");
                System.Console.Write("Type (yes) to proceed. Type anything else to cancel. ");

                string proceed = Console.ReadLine();
                if (proceed.ToLower() == "yes")
                {
                    Console.Clear();
                    using (var context = new AppDbContext())
                    {
                        var transaction = context.Database.BeginTransaction();
                        try
                        {
                            // Declares unassigned objects to later be assigned by user
                            Loan chosenLoan = null;

                            // Shows all loans that user is able to pick from
                            ListData.LoanHistory();

                            // Throws exception if there isn't any loans
                            var loans = context.Loans
                                .Include(l => l.Book)
                                .ToList();
                            if (!loans.Any())
                            {
                                throw new Exception("There are no loans as of now.");
                            }

                            // Asks user to input the ID of the loan they want to delete
                            // Throws exception if input unable to be parsed as int
                            System.Console.Write("Enter ID of loan: ");
                            string loanIdInput = Console.ReadLine();
                            int.TryParse(loanIdInput, out int chosenId);

                            // Checks if loan exists in database, if so, entity is assigned to the object
                            // If not, an exception is thrown
                            foreach (var loan in loans)
                            {
                                if (loan.loanId == chosenId)
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
                            System.Console.WriteLine($"Loan {chosenLoan.loanId} on {chosenLoan.Book.bookName} by {chosenLoan.loanerName} was successfully deleted from database!\n");
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
                    Console.Clear();
                    return;
                }
            }
            static void DeleteAll()
            {
                // DISCLAIMER Ask user if deleting everyting is necessary
                Console.Clear();
                System.Console.WriteLine("Are you sure you want to DELETE everyting? This action can't be reversed.\n");
                System.Console.Write("Type (yes) to proceed. Type anything else to cancel. ");

                string proceed = Console.ReadLine();
                if (proceed.ToLower() == "yes")
                {
                    using (var context = new AppDbContext())
                    {
                        System.Console.WriteLine("");
                        var transaction = context.Database.BeginTransaction();
                        try
                        {
                            context.Books.ExecuteDelete();
                            context.Authors.ExecuteDelete();
                            
                            Console.Clear();
                            System.Console.WriteLine("Database is now empty.\n");
                            transaction.Commit();
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
                    Console.Clear();
                    return;
                }
            }
        }
        public static void AddSeedData()
        {
            using (var context = new AppDbContext())
            {
                var transaction = context.Database.BeginTransaction();
                try
                {
                    // Creates new books and adds to Books
                    var seededBook1 = new Book
                    {
                        bookName = "Harry Potter 1",
                        datePublished = DateOnly.ParseExact("1997-06-26", "yyyy-MM-dd")
                    };
                    var seededBook2 = new Book
                    {
                        bookName = "Harry Potter 2",
                        datePublished = DateOnly.ParseExact("1999-07-06", "yyyy-MM-dd")
                    };
                    var seededBook3 = new Book
                    {
                        bookName = "IT",
                        datePublished = DateOnly.ParseExact("1986-11-15", "yyyy-MM-dd")
                    };
                    var seededBook4 = new Book
                    {
                        bookName = "Harry Potter's misery",
                        datePublished = DateOnly.ParseExact("2000-06-18", "yyyy-MM-dd")
                    };
                    var seededBook5 = new Book
                    {
                        bookName = "1984",
                        datePublished = DateOnly.ParseExact("1949-06-08", "yyyy-MM-dd")
                    };
                    context.Books.Add(seededBook1);
                    context.Books.Add(seededBook2);
                    context.Books.Add(seededBook3);
                    context.Books.Add(seededBook4);
                    context.Books.Add(seededBook5);

                    // Creates new authors and adds to Authors
                    var seededAuthor1 = new Author
                    {
                        authorName = "JK Rowling"
                    };
                    var seededAuthor2 = new Author
                    {
                        authorName = "Stephen King"
                    };
                    var seededAuthor3 = new Author
                    {
                        authorName = "George Orwell"

                    };
                    context.Authors.Add(seededAuthor1);
                    context.Authors.Add(seededAuthor2);
                    context.Authors.Add(seededAuthor3);

                    // Checking if some seed data is already provided, if so throws an excpetion
                    var books = context.Books.ToList();
                    if (books.Any(b => (b.bookName == seededBook1.bookName) || (b.bookName == seededBook2.bookName) || (b.bookName == seededBook3.bookName) || (b.bookName == seededBook4.bookName) || (b.bookName == seededBook5.bookName)))
                    {
                        throw new Exception("Seed data already exists in database.");
                    }

                    var authors = context.Authors.ToList();
                    if (authors.Any(a => (a.authorName == seededAuthor1.authorName) || (a.authorName == seededAuthor2.authorName) || (a.authorName == seededAuthor3.authorName)))
                    {
                        throw new Exception("Seed data already exists in database.");
                    }

                    context.SaveChanges();

                    // Creates BookAuthor relations and adds to BookAuthor
                    var seededBoAu1 = new BookAuthor
                    {
                        bookId = context.Books.First(b => b.bookName == "Harry Potter 1").bookId,
                        authorId = context.Authors.First(a => a.authorName == "JK Rowling").authorId
                    };
                    var seededBoAu2 = new BookAuthor
                    {
                        bookId = context.Books.First(b => b.bookName == "Harry Potter 2").bookId,
                        authorId = context.Authors.First(a => a.authorName == "JK Rowling").authorId
                    };
                    var seededBoAu3 = new BookAuthor
                    {
                        bookId = context.Books.First(b => b.bookName == "IT").bookId,
                        authorId = context.Authors.First(a => a.authorName == "Stephen King").authorId
                    };
                    var seededBoAu4 = new BookAuthor
                    {
                        bookId = context.Books.First(b => b.bookName == "Harry Potter's misery").bookId,
                        authorId = context.Authors.First(a => a.authorName == "JK Rowling").authorId
                    };
                    var seededBoAu5 = new BookAuthor
                    {
                        bookId = context.Books.First(b => b.bookName == "Harry Potter's misery").bookId,
                        authorId = context.Authors.First(a => a.authorName == "Stephen King").authorId
                    };
                    var seededBoAu6 = new BookAuthor
                    {
                        bookId = context.Books.First(b => b.bookName == "1984").bookId,
                        authorId = context.Authors.First(a => a.authorName == "George Orwell").authorId
                    };
                    context.BookAuthor.Add(seededBoAu1);
                    context.BookAuthor.Add(seededBoAu2);
                    context.BookAuthor.Add(seededBoAu3);
                    context.BookAuthor.Add(seededBoAu4);
                    context.BookAuthor.Add(seededBoAu5);
                    context.BookAuthor.Add(seededBoAu6);

                    // Saves changes to database and commit transaction if no exceptions where thrown
                    context.SaveChanges();
                    transaction.Commit();

                    System.Console.WriteLine("Database has been seeded!\n");
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
    }
}