using System;

internal class Program
{
    private static void Main(string[] args)
    {
        // DateTime dateTest = new DateTime(2024,12,3);
        // System.Console.WriteLine(dateTest.ToString("yyyy-MM-dd"));

        bool exitMainMenu = false;
        while (!exitMainMenu)
        {
            System.Console.WriteLine("MAIN MENU\n");
            System.Console.WriteLine("1 - Manage data");
            System.Console.WriteLine("2 - View data");
            System.Console.WriteLine("Q - Quit");
            System.Console.Write("\nWhat do you want to do?: ");

            string choiceInput = Console.ReadLine();
            switch (choiceInput)
            {
                case "1":
                    Console.Clear();
                    ManageDataMenu();
                    break;
                case "2":
                    Console.Clear();
                    ListDataMenu();
                    break;
                case "Q":
                case "q":
                    exitMainMenu = true;
                    break;
                default:
                    Console.Clear();
                    System.Console.WriteLine("Invalid input, try again.\nss");
                    continue;
            }
        }
    }
    public static void ManageDataMenu()
    {
        bool exitManageData = false;
        while (!exitManageData)
        {
            System.Console.WriteLine("MANAGE DATA\n");
            System.Console.WriteLine("1 - Add new book");
            System.Console.WriteLine("2 - Add new author");
            System.Console.WriteLine("3 - Add author to book");
            System.Console.WriteLine("4 - Loan a book");
            System.Console.WriteLine("5 - Delete loan, book or author");
            System.Console.WriteLine("Q - Return to main menu");
            System.Console.Write("\nWhat do you want to do?: ");

            string manageInput = Console.ReadLine();

            switch (manageInput)
            {
                // case "1":
                //     AddBook();
                //     break;
                // case "2":
                //     AddAuthor();
                //     break;
                // case "3":
                //     BookAuthorRelation();
                //     break;
                // case "4":
                //     CreateLoan();
                //     break;
                // case "5":
                //     DeleteData();
                //     break;
                case "Q":
                case "q":
                    Console.Clear();
                    exitManageData = true;
                    break;
                default:
                    Console.Clear();
                    System.Console.WriteLine("Invalid input, try again.\n");
                    continue;
            }
        }
    }

    public static void ListDataMenu()
    {
        bool exitListData = false;
        while (!exitListData)
        {
            System.Console.WriteLine("LIST DATA\n");
            System.Console.WriteLine("1 - List all books");
            System.Console.WriteLine("2 - List all books by an author");
            System.Console.WriteLine("3 - List all contributing authors to a book");
            System.Console.WriteLine("4 - List all current book loans");
            System.Console.WriteLine("5 - List book loan history");
            System.Console.WriteLine("Q - Return to main menu");
            System.Console.Write("\nWhat do you want to do?: ");

            string listInput = Console.ReadLine();

            switch (listInput)
            {
                // case "1":
                //     AllBooks();
                //     break;
                // case "2":
                //     BooksFromAuthor();
                //     break;
                // case "3":
                //     AuthorsByBook();
                //     break;
                // case "4":
                //     CurrentLoans();
                //     break;
                // case "5":
                //     LoanHistory();
                //     break;
                case "Q":
                case "q":
                    Console.Clear();
                    exitListData = true;
                    break;
                default:
                    Console.Clear();
                    System.Console.WriteLine("Invalid input, try again.\n");
                    continue;
            }
        }
    }
}