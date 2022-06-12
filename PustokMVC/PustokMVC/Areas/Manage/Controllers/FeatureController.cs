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
    public class FeatureController : Controller
    {
        private readonly AppDbContext _context;

        public FeatureController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            List<Feature> features = await _context.Features.Where(n => !n.IsDeleted).ToListAsync();

            if(features is null)
            {
                return NotFound();
            }

            return View(features);
        }

        public async Task<IActionResult> Update(int id)
        {
            Feature feature = await _context.Features.Where(n => !n.IsDeleted && n.Id == id).FirstOrDefaultAsync();

            if (feature is null)
            {
                return NotFound();
            }

            return View(feature);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int id, Feature feature)
        {
            Feature dbFeature = await _context.Features.Where(n => !n.IsDeleted && n.Id == id).FirstOrDefaultAsync();

            if (feature is null)
            {
                return NotFound();
            }

            if (dbFeature is null)
            {
                return NotFound();
            }

            if (feature.Header == null)
            {
                ModelState.AddModelError("Header", "Header cannot be empty!");
                return View(feature);
            }

            if (feature.Description == null)
            {
                ModelState.AddModelError("Description", "Description cannot be empty!");
                return View(feature);
            }

            if (feature.Icon == null)
            {
                ModelState.AddModelError("Icon", "Icon cannot be empty!");
                return View(feature);
            }

            dbFeature.Header = feature.Header;
            dbFeature.Description = feature.Description;
            dbFeature.Icon = feature.Icon;
            dbFeature.UpdatedDate = DateTime.Now;

            _context.Features.Update(dbFeature);
            await _context.SaveChangesAsync();

            return RedirectToAction(controllerName: nameof(Feature), actionName: nameof(Index));
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Feature feature)
        {
            if (feature is null)
            {
                return NotFound();
            }

            if (feature.Header == null)
            {
                ModelState.AddModelError("Header", "Header cannot be empty!");
                return View(feature);
            }

            if (feature.Header == null)
            {
                ModelState.AddModelError("Description", "Description cannot be empty!");
                return View(feature);
            }

            if (feature.Header == null)
            {
                ModelState.AddModelError("Icon", "Icon cannot be empty!");
                return View(feature);
            }

            feature.CreatedDate = DateTime.Now;

            await _context.Features.AddAsync(feature);
            await _context.SaveChangesAsync();

            return RedirectToAction(controllerName: nameof(Feature), actionName: nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            Feature feature = await _context.Features.Where(n => !n.IsDeleted && n.Id == id).FirstOrDefaultAsync();
            if (feature is null)
            {
                return NotFound();
            }

            return View(feature);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Feature feature)
        {
            Feature dbFeature = await _context.Features.Where(n => !n.IsDeleted && n.Id == feature.Id).FirstOrDefaultAsync();

            if (dbFeature is null)
            {
                return NotFound();
            }

            if (feature is null)
            {
                return NotFound();
            }

            if (feature.Header != "CONFIRM")
            {
                ModelState.AddModelError("Header", "Confirmation input doesn't match. Try again.");
                return View(feature);
            }

            dbFeature.IsDeleted = true;
            _context.Features.Update(dbFeature);
            await _context.SaveChangesAsync();
            return RedirectToAction(controllerName: nameof(Feature), actionName: nameof(Index));
        }
    }
}
