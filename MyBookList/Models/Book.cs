using MyBookList.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MyBookList.Models
{
    public class Book
    {
        public int ID { get; set; }

        [Required(ErrorMessage = "You must fill in a name to the book.")]
        public String Name { get; set; }

        [Required(ErrorMessage = "You must fill in the year the book premiered")]
        [Range(1950, 2050, ErrorMessage = "It must be between 1950 and 2017")]
        public int Year { get; set; }

        [Required(ErrorMessage = "You must fill in the synopsis of the book")]
        public String Synopsis { get; set; }

        public String BookImagePath { get; set; }

        public void SaveImage(HttpPostedFileBase image, String serverPath, String pathToFile)
        {
            if (image == null) return;

            //ImageModel
            Guid guid = Guid.NewGuid();
            ImageModel.ResizeAndSave(serverPath + pathToFile, guid.ToString(), image.InputStream, 275);

            BookImagePath = pathToFile + guid.ToString() + ".jpg";
        }

        public virtual ICollection<BookGenre> BooksGenres { get; set; }

        public virtual ICollection<BookAuthor> BooksAuthors { get; set; }

    }
}
