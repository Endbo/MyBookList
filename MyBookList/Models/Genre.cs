using MyBookList.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MyBookList.Models
{
    public class Genre
    {
        public int ID { get; set; }

        [Required(ErrorMessage = "You must fill in the genre name of the the book.")]
        public String Name { get; set; }

        public virtual ICollection<BookGenre> BooksGenres { get; set; }

    }
}