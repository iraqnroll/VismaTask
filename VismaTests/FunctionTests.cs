using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using VismaTask.Classes;
using NodaTime;
using System.Linq;

namespace VismaTests
{
    [TestClass]
    public class FunctionTests
    {
        [TestMethod]
        public void Add_AddsBookWithID()
        {
            List<Book> Books = new List<Book>();
            LibraryManager Manager = LibraryManager.Instance;
            Book TestBook = new Book("name", "author", "category", "language", new LocalDate(2021, 5, 15), "isbn");

            Manager.AddBook(Books, TestBook);
            Manager.AddBook(Books, TestBook);

            TestBook.Id = 0;
            Assert.AreEqual(Books.First(), TestBook);
            TestBook.Id = 1;
            Assert.AreEqual(Books.Last(), TestBook);
        }
        [TestMethod]
        public void Take_AssignsReader()
        {
            List<Book> Books = new List<Book>();
            LibraryManager Manager = LibraryManager.Instance;
            Book TestBook = new Book("name", "author", "category", "language", new LocalDate(2021, 5, 15), "isbn");
            LibraryReader TestReader = new LibraryReader("Vardenis", "Pavardenis", new LocalDate(2021, 5, 20));

            Manager.AddBook(Books, TestBook);
            Manager.TakeBook(Books, 0, TestReader);

            Assert.AreEqual(Books.First().Reader, TestReader);
        }
        [TestMethod]
        public void Take_DateOver2Months_ShouldThrowTakenDatePeriod()
        {
            List<Book> Books = new List<Book>();
            LibraryManager Manager = LibraryManager.Instance;
            Book TestBook = new Book("name", "author", "category", "language", new LocalDate(2021, 5, 15), "isbn");
            LibraryReader TestReader = new LibraryReader("Vardenis", "Pavardenis", new LocalDate(2023, 5, 20));
            bool ExceptionWasThrown = false;

            try
            {
                Manager.AddBook(Books, TestBook);
                Manager.TakeBook(Books, 0, TestReader);
            }
            catch (TakeDatePeriodException e)
            {
                ExceptionWasThrown = true;
            }

            Assert.IsTrue(ExceptionWasThrown);
        }
        [TestMethod]
        public void Take_TakingMoreThan3Books_ShouldThrowTakenBooksLimit()
        {
            const int BookAmount = 4;
            List<Book> Books = new List<Book>();
            LibraryManager Manager = LibraryManager.Instance;
            Book TestBook = new Book("name", "author", "category", "language", new LocalDate(2021, 5, 15), "isbn");
            LibraryReader TestReader = new LibraryReader("Vardenis", "Pavardenis", new LocalDate(2021, 5, 20));
            bool ExceptionWasThrown = false;

            try
            {
                for (uint i = 0; i < BookAmount; i++)
                {
                    Manager.AddBook(Books, TestBook);
                    Manager.TakeBook(Books, i, TestReader);
                }
            }
            catch (TakenBooksLimitException e)
            {
                ExceptionWasThrown = true;
            }

            Assert.IsTrue(ExceptionWasThrown);
        }
    }
}
