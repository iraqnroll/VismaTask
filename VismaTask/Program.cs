using System;
using VismaTask.Classes;
using NodaTime;
using System.IO;
using System.Collections.Generic;

namespace VismaTask
{
    class Program
    {
        static void DisplayHelp()
        {
            Console.WriteLine("-h\t Display Help screen.");
            Console.WriteLine("-a [Name] [Author] [Category] [Language] [Date published (yyyy-MM-dd)] [ISBN]\t Add a new book to the library.");
            Console.WriteLine("-t [Book ID] [Your name] [Your surname] [Return date (yyyy-MM-dd)]\t Take a book from the library.");
            Console.WriteLine("-r [Book ID] [Your name] [Your surname]\t Return a book to the library.");
            Console.WriteLine("-l [ Author | Category | Language | ISBN | Name | Taken | Available ] [Filter value]\t List all the books with the specified filter.");
            Console.WriteLine("-d [Book ID]\t Delete a book from the library.");
        }
        static void Main(string[] args)
        {
            string fileName = "Books.json";
            string filePath = Path.Combine(Environment.CurrentDirectory, fileName);
            LibraryManager Manager = LibraryManager.Instance;
            Book newBook;
            LibraryReader reader;
            List<Book> Books;

            Filter currentFilter;

            if (!File.Exists(filePath))
            {
                File.Create(filePath).Close();
            }
            try
            {
                switch (args[0])
                {
                    case "-h":
                        DisplayHelp();
                        break;
                    case "-a":
                        newBook = new Book(args[1], args[2], args[3], args[4], Manager.ParseDate(args[5]), args[6]);
                        Books = Manager.GetBooks(filePath);
                        Manager.AddBook(Books, newBook);
                        Manager.SetBooks(Books, filePath);
                        break;
                    case "-t":
                        reader = new LibraryReader(args[2], args[3], Manager.ParseDate(args[4]));
                        Books = Manager.GetBooks(filePath);
                        Manager.TakeBook(Books, (uint)Int32.Parse(args[1]), reader);
                        Manager.SetBooks(Books, filePath);
                        break;
                    case "-r":
                        reader = new LibraryReader(args[2], args[3]);
                        Books = Manager.GetBooks(filePath);
                        Manager.ReturnBook(Books, (uint)Int32.Parse(args[1]), reader);
                        Manager.SetBooks(Books, filePath);
                        break;
                    case "-l":
                        if (Enum.TryParse(args[1], out currentFilter))
                        {
                            Books = Manager.GetBooks(filePath);
                            if (args.Length == 2)
                            {
                                Console.WriteLine(Manager.ListBooks(Books, currentFilter, null));
                            }
                            else
                            {
                                Console.WriteLine(Manager.ListBooks(Books, currentFilter, args[2]));
                            }
                        }
                        break;
                    case "-d":
                        Books = Manager.GetBooks(filePath);
                        Manager.DeleteBook(Books, (uint)Int32.Parse(args[1]));
                        Manager.SetBooks(Books, filePath);
                        break;
                    default:
                        Console.WriteLine("Invalid arguments.\n");
                        break;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

        }
    }
}
