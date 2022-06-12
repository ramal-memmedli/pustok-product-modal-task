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
    public class GenreController : Controller
    {
        private readonly AppDbContext _context;

        public GenreController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            List<Genre> genres = await _context.Genres.Where(n => !n.IsDeleted).ToListAsync();

            if (genres is null)
            {
                return NotFound();
            }

            return View(genres);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Genre genre)
        {
            if (genre.Name is null)
            {
                ModelState.AddModelError("Name", "This field cannot be empty!");
                return View(genre);
            }

            genre.CreatedDate = DateTime.Now;

            await _context.Genres.AddAsync(genre);
            await _context.SaveChangesAsync();

            return RedirectToAction(controllerName: nameof(genre), actionName: nameof(Index));
        }

        public async Task<IActionResult> Update(int id)
        {
            Genre genre = await _context.Genres.Where(n => !n.IsDeleted && n.Id == id).FirstOrDefaultAsync();

            if (genre is null)
            {
                return NotFound();
            }

            return View(genre);
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Update(Genre genre)
        {
            Genre dbGenre = await _context.Genres.Where(n => !n.IsDeleted && n.Id == genre.Id).FirstOrDefaultAsync();

            if (dbGenre is null)
            {
                return NotFound();
            }

            if (genre.Name is null)
            {
                ModelState.AddModelError("Name", "This field cannot be empty!");
                return View(genre);
            }

            dbGenre.Name = genre.Name;
            dbGenre.UpdatedDate = DateTime.Now;

            _context.Genres.Update(dbGenre);
            await _context.SaveChangesAsync();

            return RedirectToAction(controllerName: nameof(genre), actionName: nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            Genre genre = await _context.Genres.Where(n => !n.IsDeleted && n.Id == id).FirstOrDefaultAsync();

            if (genre is null)
            {
                return NotFound();
            }

            return View(genre);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Genre genre)
        {
            Genre dbGenre = await _context.Genres.Where(n => !n.IsDeleted && n.Id == genre.Id).FirstOrDefaultAsync();

            if (dbGenre is null)
            {
                return NotFound();
            }

            if (genre is null)
            {
                return NotFound();
            }

            if (genre.Name != "CONFIRM")
            {
                ModelState.AddModelError("Name", "Confirmation input doesn't match!");
                return View(genre);
            }

            dbGenre.IsDeleted = true;

            _context.Genres.Update(dbGenre);
            await _context.SaveChangesAsync();

            return RedirectToAction(controllerName: nameof(genre), actionName: nameof(Index));
        }
    }
}
