using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BookApiCore.Model
{
    public class Book
    {
        public String Title;
        public String[] Authors;
        public String PublishDate;
        public String Description;
        public String[] Categories;
        public Dictionary<string, string> ImageLinks;
        public String Language;

        public Book(string title, string[] authors, string publishDate, string description, string[] categories, Dictionary<string, string> imageLinks, string language)
        {
            Title = title;
            Authors = authors;
            PublishDate = publishDate;
            Description = description;
            Categories = categories;
            ImageLinks = imageLinks;
            Language = language;
        }

        public Book()
        {
            
        }
    }
    
}
