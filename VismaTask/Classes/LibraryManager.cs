using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using System.Text;
using NodaTime;
using System.Threading.Tasks;
using System.Globalization;

namespace VismaTask.Classes
{
    public sealed class LibraryManager
    {
        private static readonly LibraryManager instance = new LibraryManager();
        private LibraryManager() { }
        public static LibraryManager Instance
        {
            get
            {
                return instance;
            }
        }

        private const int TakenBooksLimit = 3;
        private const int BorrowMonthPeriodLimit = 2;

        public LocalDate ParseDate(string Date)
        {
            string[] elements = Date.Split('-');
            return new LocalDate(Int32.Parse(elements[0]), Int32.Parse(elements[1]), Int32.Parse(elements[2]));
        }
        private static string OutputBook(Book book)
        {
            var Taken = book.Reader;
            if (Taken != null)
            {
                return String.Format("{0}\t {1}\t - {2}\t {3}\t {4}\t {5}\t {6}\t [Taken]\n",
                    book.Id, book.Author, book.Name, book.PublicationDate.ToString("d", CultureInfo.InvariantCulture), book.Language, book.Category, book.Isbn);
            }
            else
            {
                return String.Format("{0}\t {1} - {2}\t {3} {4} {5} {6} [Available]\n",
                    book.Id, book.Author, book.Name, book.PublicationDate.ToString("d", CultureInfo.InvariantCulture), book.Language, book.Category, book.Isbn);
            }
        }
        private LocalDate GetCurrentDate()
        {
            var now = SystemClock.Instance.GetCurrentInstant();
            var tz = DateTimeZoneProviders.Bcl.GetSystemDefault();
            var zdt = now.InZone(tz);
            return zdt.LocalDateTime.Date;
        }
        public List<Book> GetBooks(string jsonFile)
        {
            string jsonString;

            using (StreamReader reader = new StreamReader(jsonFile))
            {
                jsonString = reader.ReadToEnd();
            }
            if (jsonString.Length > 0)
            {
                return JsonConvert.DeserializeObject<List<Book>>(jsonString);
            }
            else
            {
                return new List<Book>();
            }
        }
        public void SetBooks(List<Book> Books, string jsonFile)
        {
            string jsonString;

            using (StreamWriter writer = new StreamWriter(jsonFile))
            {
                jsonString = JsonConvert.SerializeObject(Books);
                writer.Write(jsonString);
            }
        }

        public void AddBook(List<Book> Books, Book newBook)
        {
            if (Books.Count() >= 1)
            {
                var lastBook = Books.Last();
                newBook.Id = lastBook.Id + 1;
                Books.Add(newBook);
            }
            else
            {
                newBook.Id = 0;
                Books.Add(newBook);
            }
            Console.WriteLine("Book has been successfuly added !\n");
        }

        public void DeleteBook(List<Book> Books, uint Id)
        {
            if (Books.Count() > 0)
            {
                var deletedBook = Books.SingleOrDefault(b => b.Id == Id);

                if (deletedBook != null)
                {
                    Books.Remove(deletedBook);
                    Console.WriteLine("Book has been successfuly deleted !\n");
                }
                else
                {
                    throw new BookNotFoundException(Id);
                }
            }
            else
            {
                throw new EmptyLibraryException();
            }
        }

        public void TakeBook(List<Book> Books, uint Id, LibraryReader libReader)
        {
            libReader.PickupDate = GetCurrentDate();
            var takenBooks = Books.Where(x => x.Reader != null).ToList();

            if (Books.Count() > 0)
            {
                if (takenBooks.Where(x => x.Reader.Equals(libReader)).Count() >= TakenBooksLimit)
                {
                    throw new TakenBooksLimitException();
                }
                else
                {
                    var dateSpan = Period.Between(libReader.PickupDate, libReader.ReturnDate, PeriodUnits.Months);
                    if (dateSpan.Months > BorrowMonthPeriodLimit)
                    {
                        throw new TakeDatePeriodException();
                    }
                    else
                    {
                        foreach (var book in Books.Where(b => b.Id == Id))
                        {
                            book.Reader = libReader;
                        }
                        Console.WriteLine("Succesfully took the book !\n");
                    }
                }
            }
            else
            {
                throw new EmptyLibraryException();
            }

        }

        public void ReturnBook(List<Book> Books, uint Id, LibraryReader libReader)
        {
            var returnedBook = Books.SingleOrDefault(b => b.Id == Id);

            if (Books.Count() > 0)
            {
                if (returnedBook.Reader != null && returnedBook.Reader.Equals(libReader))
                {
                    var currentDate = GetCurrentDate();
                    if (returnedBook.Reader.ReturnDate.CompareTo(currentDate) < 0)
                    {
                        Console.WriteLine("You're late to return the book ! -rep.\n");
                    }
                    foreach (var book in Books.Where(b => b.Id == Id))
                    {
                        book.Reader = null;
                    }
                    Console.WriteLine("Succesfully returned the book !\n");
                }
                else
                {
                    throw new NotReadersBookException();
                }
            }
            else
            {
                throw new EmptyLibraryException();
            }

        }

        public string ListBooks(List<Book> Books, Filter filter, string filterArg)
        {
            string OutputString;
            if (Books.Count() > 0)
            {
                OutputString = "ID\t Author\t Book Name\t Date\t Language\t Category\t ISBN\t Availability\n";

                switch (filter)
                {
                    case Filter.Author:
                        foreach (var book in Books.Where(b => b.Author == filterArg))
                        {
                            OutputString += OutputBook(book);
                        }
                        break;
                    case Filter.Category:
                        foreach (var book in Books.Where(b => b.Category == filterArg))
                        {
                            OutputString += OutputBook(book);
                        }
                        break;
                    case Filter.Language:
                        foreach (var book in Books.Where(b => b.Language == filterArg))
                        {
                            OutputString += OutputBook(book);
                        }
                        break;
                    case Filter.ISBN:
                        foreach (var book in Books.Where(b => b.Isbn == filterArg))
                        {
                            OutputString += OutputBook(book);
                        }
                        break;
                    case Filter.Name:
                        foreach (var book in Books.Where(b => b.Name == filterArg))
                        {
                            OutputString += OutputBook(book);
                        }
                        break;
                    case Filter.Taken:
                        foreach (var book in Books.Where(b => b.Reader != null))
                        {
                            OutputString += OutputBook(book);
                        }
                        break;
                    case Filter.Available:
                        foreach (var book in Books.Where(b => b.Reader == null))
                        {
                            OutputString += OutputBook(book);
                        }
                        break;
                    default:
                        throw new InvalidFilterException();
                        break;
                }
            }
            else
            {
                throw new EmptyLibraryException();
            }
            return OutputString;
        }
    }
}
