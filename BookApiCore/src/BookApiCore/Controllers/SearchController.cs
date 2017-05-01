using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookApiCore.Helpers;
using BookApiCore.Model;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace BookApiCore.Controllers
{
    [Route("api/[controller]")]
    public class SearchController : Controller
    {
        public GoogleBookInteracton test = new GoogleBookInteracton();
        // GET: /<controller>/
        public string Index()
        {
            return "hello World";
        }
        // GET api/values/5
        [HttpGet("{id}")]
        public async Task<List<Book>> Get(string id)
        {
            return await test.DownloadBooks(id, 5);
        }
    }
}
