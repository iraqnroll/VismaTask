using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NodaTime;
using System.Threading.Tasks;

namespace VismaTask.Classes
{
    public class LibraryReader : IEquatable<LibraryReader>
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public LocalDate PickupDate { get; set; }
        public LocalDate ReturnDate { get; set; }

        public LibraryReader()
        {
        }
        public LibraryReader(string Name, string Surname)
        {
            this.Name = Name;
            this.Surname = Surname;
        }
        public LibraryReader(string Name, string Surname, LocalDate ReturnDate)
        {
            this.Name = Name;
            this.Surname = Surname;
            this.ReturnDate = ReturnDate;
        }

        public bool Equals(LibraryReader other)
        {
            if (other == null)
                return false;
            if (this.Name == other.Name && this.Surname == other.Surname)
            {
                return true;
            }
            else
                return false;
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            LibraryReader readerObj = obj as LibraryReader;
            if (readerObj == null)
                return false;
            else
                return Equals(readerObj);
        }

        public override int GetHashCode()
        {
            return this.Surname.GetHashCode();
        }
    }
}
