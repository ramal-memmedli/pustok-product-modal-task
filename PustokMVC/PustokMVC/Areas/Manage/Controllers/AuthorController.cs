using Data.DAL;
using Data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PustokMVC.Areas.Manage.Controllers
{
    public class AuthorController : Controller
    {
        private readonly AppDbContext _context;

        public AuthorController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            List<Author> authors = await _context.Authors.Where(n => !n.IsDeleted).ToListAsync();

            if(authors is null)
            {
                return NotFound();
            }

            return View(authors);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Author author)
        {
            if(author.FullName is null)
            {
                ModelState.AddModelError("FullName", "This field cannot be empty!");
                return View(author);
            }

            author.CreatedDate = DateTime.Now;

            await _context.Authors.AddAsync(author);
            await _context.SaveChangesAsync();

            return RedirectToAction(controllerName: nameof(author), actionName: nameof(Index));
        }

        public async Task<IActionResult> Update(int id)
        {
            Author author = await _context.Authors.Where(n => !n.IsDeleted && n.Id == id).FirstOrDefaultAsync();

            if(author is null)
            {
                return NotFound();
            }

            return View(author);
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Update(Author author)
        {
            Author dbAuthor = await _context.Authors.Where(n => !n.IsDeleted && n.Id == author.Id).FirstOrDefaultAsync();

            if (dbAuthor is null)
            {
                return NotFound();
            }

            if(author.FullName is null)
            {
                ModelState.AddModelError("FullName", "This field cannot be empty!");
                return View(author);
            }

            dbAuthor.FullName = author.FullName;
            dbAuthor.UpdatedDate = DateTime.Now;

            _context.Authors.Update(dbAuthor);
            await _context.SaveChangesAsync();

            return RedirectToAction(controllerName: nameof(author), actionName: nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            Author author = await _context.Authors.Where(n => !n.IsDeleted && n.Id == id).FirstOrDefaultAsync();

            if (author is null)
            {
                return NotFound();
            }

            return View(author);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Author author)
        {
            Author dbAuthor = await _context.Authors.Where(n => !n.IsDeleted && n.Id == author.Id).FirstOrDefaultAsync();

            if (dbAuthor is null)
            {
                return NotFound();
            }

            if(author is null)
            {
                return NotFound();
            }

            if(author.FullName != "CONFIRM")
            {
                ModelState.AddModelError("FullName", "Confirmation input doesn't match!");
                return View(author);
            }

            dbAuthor.IsDeleted = true;

            _context.Authors.Update(dbAuthor);
            await _context.SaveChangesAsync();

            return RedirectToAction(controllerName: nameof(author), actionName: nameof(Index));
        }
    }
}
