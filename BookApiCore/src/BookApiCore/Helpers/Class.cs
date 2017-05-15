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
            try
            {
                HttpClient client = new HttpClient();
                string query_string = string.Format("https://www.googleapis.com/books/v1/volumes?maxResults={1}&q={0}&key=AIzaSyA3lKpqzD_6LDx3I1uglIyIMLq6gB0HvjA", bookName, amount);
                var byteArray = await client.GetByteArrayAsync(query_string);
                var bookVolume = JsonConvert.DeserializeObject<dynamic>(System.Text.Encoding.UTF8.GetString(byteArray, 0, byteArray.Length));

                var books = new List<Book>();
                //loops over each book and requests there book
                foreach (var item in bookVolume.items)
                {
                    var selfLink = (string)item["selfLink"].ToString();
                    if (selfLink == null)
                    {
                        continue;
                    }
                    //should most likely not await here but it works so changing it seemed like a bad idea
                    var book = await ProcessBookUrl(selfLink, client);
                    books.Add(book);
                }
                return books;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            
        }
        /// <summary>
        /// requests the specified url and returns a book 
        /// </summary>
        /// <param name="url"></param>
        /// <param name="webClient"></param>
        /// <returns></returns>
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
