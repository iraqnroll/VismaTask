using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VismaTask.Classes
{
    [Serializable]
    public class BookNotFoundException : Exception
    {
        public BookNotFoundException() { }
        public BookNotFoundException(uint BookID)
            : base(String.Format("Book with ID {0} doesn't exist.", BookID)) { }
    }
    [Serializable]
    public class EmptyLibraryException : Exception
    {
        public EmptyLibraryException()
            : base("The library is empty.") { }
    }
    [Serializable]
    public class TakenBooksLimitException : Exception
    {
        public TakenBooksLimitException()
            : base("The 3 taken books limit has been reached.") { }
    }
    [Serializable]
    public class TakeDatePeriodException : Exception
    {
        public TakeDatePeriodException()
            : base("Maximum allowed period to take a book for is 2 months") { }
    }
    [Serializable]
    public class NotReadersBookException : Exception
    {
        public NotReadersBookException()
            : base("This is not one of the reader's borrowed books.") { }
    }
    [Serializable]
    public class InvalidFilterException : Exception
    {
        public InvalidFilterException()
            : base("Incorrect filter.") { }
    }
}
