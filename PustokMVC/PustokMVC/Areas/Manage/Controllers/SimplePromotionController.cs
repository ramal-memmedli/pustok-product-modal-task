using Data.DAL;
using Data.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace PustokMVC.Areas.Manage.Controllers
{
    public class SimplePromotionController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _environment;

        public SimplePromotionController(AppDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        public async Task<IActionResult> Index()
        {
            List<SimplePromotion> simplePromotions = await _context.SimplePromotions.Where(n => !n.IsDeleted).ToListAsync();

            if (simplePromotions is null)
            {
                return NotFound();
            }

            return View(simplePromotions);
        }

        public async Task<IActionResult> Update(int id)
        {
            SimplePromotion simplePromotion = await _context.SimplePromotions.Where(n => !n.IsDeleted && n.Id == id).FirstOrDefaultAsync();

            if (simplePromotion is null)
            {
                return NotFound();
            }

            return View(simplePromotion);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int id, SimplePromotion promotion)
        {
            SimplePromotion simplePromotion = await _context.SimplePromotions.Where(n => !n.IsDeleted && n.Id == id).FirstOrDefaultAsync();

            if (simplePromotion is null)
            {
                return NotFound();
            }

            if (promotion is null)
            {
                return NotFound();
            }

            if (promotion.RedirectUrl == null || string.IsNullOrEmpty(promotion.RedirectUrl.Trim()))
            {
                ModelState.AddModelError("RedirectUrl", "This field cannot be an empty!");
                return View(promotion);
            }

            if (promotion.ImageFile != null)
            {
                decimal size = (decimal)promotion.ImageFile.Length / 1024 / 1024;

                if (size > 3)
                {
                    ModelState.AddModelError("ImageFile", "Max allowed image size is 3MB!");
                    return View(promotion);
                }

                if (!promotion.ImageFile.ContentType.Contains("image/"))
                {
                    ModelState.AddModelError("ImageFile", "Other file types except image not allowed!");
                    return View(promotion);
                }

                string imageName = promotion.ImageFile.FileName;

                if (imageName.Length > 218)
                {
                    imageName = imageName.Substring(imageName.Length - 218, 218);
                }

                imageName = Guid.NewGuid().ToString() + imageName;

                string path = Path.Combine(_environment.WebRootPath, "assets", "uploads", "promotionImages", imageName);

                using (FileStream fileStream = new FileStream(path, FileMode.Create))
                {
                    await promotion.ImageFile.CopyToAsync(fileStream);
                }

                simplePromotion.Image = imageName;
            }

            simplePromotion.RedirectUrl = promotion.RedirectUrl;
            simplePromotion.UpdatedDate = DateTime.Now;
            _context.SimplePromotions.Update(simplePromotion);
            await _context.SaveChangesAsync();

            return RedirectToAction(controllerName: nameof(SimplePromotion), actionName: nameof(Index));
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SimplePromotion promotion)
        {
            if (promotion is null)
            {
                return NotFound();
            }

            if (promotion.RedirectUrl == null || string.IsNullOrEmpty(promotion.RedirectUrl.Trim()))
            {
                ModelState.AddModelError("RedirectUrl", "This field cannot be an empty!");
                return View(promotion);
            }

            if (promotion.ImageFile is null)
            {
                ModelState.AddModelError("ImageFile", "This field cannot be an empty!");
                return View(promotion);
            }

            decimal size = (decimal)promotion.ImageFile.Length / 1024 / 1024;

            if (size > 3)
            {
                ModelState.AddModelError("ImageFile", "Max allowed image size is 3MB!");
                return View(promotion);
            }

            if (!promotion.ImageFile.ContentType.Contains("image/"))
            {
                ModelState.AddModelError("ImageFile", "Other file types except image not allowed!");
                return View(promotion);
            }

            string imageName = promotion.ImageFile.FileName;

            if (imageName.Length > 218)
            {
                imageName = imageName.Substring(imageName.Length - 218, 218);
            }

            imageName = Guid.NewGuid().ToString() + imageName;

            string path = Path.Combine(_environment.WebRootPath, "assets", "uploads", "promotionImages", imageName);

            using (FileStream fileStream = new FileStream(path, FileMode.Create))
            {
                await promotion.ImageFile.CopyToAsync(fileStream);
            }

            promotion.Image = imageName;
            promotion.CreatedDate = DateTime.Now;
            await _context.SimplePromotions.AddAsync(promotion);
            await _context.SaveChangesAsync();

            return RedirectToAction(controllerName: nameof(SimplePromotion), actionName: nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            SimplePromotion promotion = await _context.SimplePromotions.Where(n => !n.IsDeleted && n.Id == id).FirstOrDefaultAsync();

            if (promotion is null)
            {
                return NotFound();
            }

            return View(promotion);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(SimplePromotion promotion)
        {
            SimplePromotion dbPromotion = await _context.SimplePromotions.Where(n => !n.IsDeleted && n.Id == promotion.Id).FirstOrDefaultAsync();

            if (dbPromotion is null)
            {
                return NotFound();
            }

            if (promotion is null)
            {
                return NotFound();
            }

            if (promotion.RedirectUrl != "CONFIRM")
            {
                ModelState.AddModelError("RedirectUrl", "Confirmation input doesn't match. Try again.");
                return View(promotion);
            }

            dbPromotion.IsDeleted = true;
            _context.SimplePromotions.Update(dbPromotion);
            await _context.SaveChangesAsync();
            return RedirectToAction(controllerName: nameof(SimplePromotion), actionName: nameof(Index));
        }
    }
}
