using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MyBookList.Models;

namespace MyBookList.Controllers
{
    public class BooksController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Books
        public ActionResult Index(String searchString)
        {

            // Er en LINQ statement som gør at man kan søge efter film
            var books = from m in db.Books
                        select m;

            if (!String.IsNullOrEmpty(searchString))
            {
                books = db.Books.Where(s => s.Name.Contains(searchString));
            }

            return View(books.OrderBy(s => s.Name).ToList());
        }

        // GET: Books/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Book book = db.Books.Find(id);

            if (book == null)
            {
                return HttpNotFound();
            }
            return View(book);
        }

        // GET: Books/Create
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Books/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Create(Book book, HttpPostedFileBase image)
        {
            if (ModelState.IsValid)
            {
                db.Books.Add(book);
                book.SaveImage(image, Server.MapPath("~"), "/BookImages/");

                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(book);
        }

        // GET: Books/Edit/5
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Book book = db.Books.Find(id);
            if (book == null)
            {
                return HttpNotFound();
            }
            return View(book);
        }

        // POST: Books/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(Book book, HttpPostedFileBase image)
        {
            if (ModelState.IsValid)
            {
                // Rediger den valgte Book
                db.Entry(book).State = EntityState.Modified;
                book.SaveImage(image, Server.MapPath("~"), "/BookImages/");

                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(book);
        }

        // GET: Books/Delete/5
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Book book = db.Books.Find(id);
            if (book == null)
            {
                return HttpNotFound();
            }
            return View(book);
        }

        // POST: Books/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteConfirmed(int id)
        {
            Book book = db.Books.Find(id);
            db.Books.Remove(book);
            db.SaveChanges();
            return RedirectToAction("Index");
        }



        //------------- Create Genre to a Book - Start-----------------------------------------------------------------------------------------------------------------------------------------------


        // GET: BookGenre/Create
        [Authorize(Roles = "Admin")]
        public ActionResult CreateGenretoBook(int? bookID)
        {
            ViewBag.BookID = db.Books.Find(bookID);
            ViewBag.GenreID = new SelectList(db.Genres, "ID", "Name");
            return View();
        }

        // POST: BookGenre/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult CreateGenretoBook(BookGenre bookGenre)
        {
            if (ModelState.IsValid)
            {
                db.BooksGenres.Add(bookGenre);

                // Validering/Tjekker om der allerede eksister et genre til den valgte Book 
                var bookGenreValidation = db.BooksGenres.Where(s => s.BookID == bookGenre.BookID && s.GenreID == bookGenre.GenreID).FirstOrDefault();

                if (bookGenreValidation == null)
                {
                    db.SaveChanges();
                    return RedirectToAction("Edit/" + bookGenre.BookID);
                }
                else
                {
                    ModelState.AddModelError("", "That Genre exist already in the Book");
                }

            }

            ViewBag.GenreID = new SelectList(db.Genres, "ID", "Name", bookGenre.GenreID);
            return View(bookGenre);
        }


        //------------- Create Genre to a Book - Done-----------------------------------------------------------------------------------------------------------------------------------------------




        //------------- Create Author to a Book - Start-----------------------------------------------------------------------------------------------------------------------------------------------


        // GET: BookAuthor/Create
        [Authorize(Roles = "Admin")]
        public ActionResult CreateAuthorToBook(int? authorID)
        {
            ViewBag.BookID = db.Books.Find(authorID);
            ViewBag.AuthorID = new SelectList(db.Authors, "ID", "FullName");
            return View();
        }

        // POST: BookAuthor/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult CreateAuthorToBook(BookAuthor bookAuthor)
        {
            if (ModelState.IsValid)
            {
                db.BooksAuthors.Add(bookAuthor);

                // Validering/Tjekker om der allerede eksister et author til den valgte Book 
                var bookAuthorValidation = db.BooksAuthors.Where(s => s.BookID == bookAuthor.BookID && s.AuthorID == bookAuthor.AuthorID).FirstOrDefault();

                if (bookAuthorValidation == null)
                {
                    db.SaveChanges();
                    return RedirectToAction("Edit/" + bookAuthor.BookID);
                }
                else
                {
                    ModelState.AddModelError("", "That Author exist already in the Book");
                }

            }

            ViewBag.AuthorID = new SelectList(db.Authors, "ID", "FullName", bookAuthor.AuthorID);
            return View(bookAuthor);
        }


        //------------- Create Author to a Book - Done-----------------------------------------------------------------------------------------------------------------------------------------------

    }
}
