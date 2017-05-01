using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using BookApiCore.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

namespace BookApiCore.Helpers
{
    public class GoogleBookInteracton
    {
        /// <summary>
        /// returns the merged responses for each google call required to show 
        /// </summary>
        /// <param name="bookName">The desired search phrase not limited to bookName as the param might suggest</param>
        /// <param name="amount"> int designating the max number of results to return</param>
        /// <returns></returns>
        public async Task<List<Book>>  DownloadBooks(string bookName, int amount)
        {
            HttpClient client = new HttpClient();
            string query_string = System.Net.WebUtility.HtmlEncode(bookName);
            var byteArray = await client.GetByteArrayAsync(string.Format("https://www.googleapis.com/books/v1/volumes?q={0}&maxResults={1}", query_string, amount));
            var bookVolume = JsonConvert.DeserializeObject<dynamic>(System.Text.Encoding.UTF8.GetString(byteArray, 0, byteArray.Length));

            var books = new List<Book>();
            foreach (var item in bookVolume.items)
            {
                var selfLink = (string) item["selfLink"].ToString();
                if (selfLink == null)
                {
                    continue;
                }
                var book = await ProcessBookUrl(selfLink, client);
                books.Add(book);   
            }
            return books;
        }

        private async Task<Book> ProcessBookUrl(string url, HttpClient webClient)
        {
            var byteArray = await webClient.GetByteArrayAsync(url);
            var JBook = JsonConvert.DeserializeObject<dynamic>(System.Text.Encoding.UTF8.GetString(byteArray, 0, byteArray.Length));

            Dictionary<string,string> imagelinks = new Dictionary<string, string>();
            foreach (var image in JBook.volumeInfo.imageLinks)
            {
                if (image == null)
                {
                    continue;
                }
                imagelinks.Add(image.Name.ToString(), image.Value.ToString());
            }
            string[] authors;
            if (JBook.volumeInfo.authors == null)
            {
                authors = new string[0];
            }
            else
            {
                authors = JBook.volumeInfo.authors.ToObject<string[]>();
            }
            string[] cat;
            if (JBook.volumeInfo.categories == null)
            {
                cat = new string[0];
            }
            else
            {
                cat = JBook.volumeInfo.categories.ToObject<string[]>();
            }
            var book = new Book(
                    (JBook.volumeInfo.title ?? "").ToString(),
                    authors,
                    (JBook.volumeInfo.publishedDate ?? "").ToString(),
                    (JBook.volumeInfo.description ?? "").ToString(),
                    cat,
                    imagelinks,
                    (JBook.volumeInfo.language ?? "").ToString()
                );
            
            
            return book;
        }
        private async Task<JObject> ProcessUrl(string url, HttpClient webClient)
        {
            var byteArray = await webClient.GetByteArrayAsync(url);
            return JObject.Parse(System.Text.Encoding.UTF8.GetString(byteArray, 0, byteArray.Length));
        }   
    }
}
