using MyBookList.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyBookList.Models
{
    public class BookGenre
    {
        public int ID { get; set; }

        public int BookID { get; set; }
        public virtual Book Book { get; set; }

        public int GenreID { get; set; }
        public virtual Genre Genre { get; set; }
    }
}