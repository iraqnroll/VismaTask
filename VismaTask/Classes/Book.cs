using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NodaTime;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace VismaTask.Classes
{
    public class Book : IEquatable<Book>
    {
        public uint Id { get; set; }
        public string Name { get; set; }
        public string Author { get; set; }
        public string Category { get; set; }
        public string Language { get; set; }
        public LocalDate PublicationDate { get; set; }
        public string Isbn { get; set; }
        [JsonProperty]
        public LibraryReader Reader { get; set; }

        public Book(string Name, string Author, string Category, string Language, LocalDate Publication, string Isbn)
        {
            this.Name = Name;
            this.Author = Author;
            this.Category = Category;
            this.Language = Language;
            this.PublicationDate = Publication;
            this.Isbn = Isbn;
            this.Reader = null;
        }

        public bool Equals(Book other)
        {
            if (other == null)
                return false;
            if (this.Name == other.Name && this.Author == other.Author && this.Language == other.Language && this.Isbn == other.Isbn)
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
            Book bookObj = obj as Book;
            if (bookObj == null)
                return false;
            else
                return Equals(bookObj);
        }

        public override int GetHashCode()
        {
            return this.Isbn.GetHashCode();
        }
    }
}
