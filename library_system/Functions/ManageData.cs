using System;

namespace library_system.Functions
{
    public class ManageData
    {
        public static void AddBook()
        {
            System.Console.WriteLine("ADDING NEW BOOK\n");

            System.Console.Write("Enter name of book: ");
            string nameInput = Console.ReadLine();

            // FORTSÄTT HÄRIFRÅN

            System.Console.Write("Enter the year the book was published (yyyy-mm-dd)");
            string format = "yyyy-MM-dd";
            string yearInput = DateOnly.ParseExact(Console.ReadLine(), format);

            using (var context = new AppDbContext())
            {
                var transaction = context.Database.BeginTransaction();
                try
                {
                    if (string.is)
                }
                catch (Exception ex)
                {

                }
            }
        }
        public static void AddAuthor()
        {
            
        }
        public static void  BookAuthorRelation()
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