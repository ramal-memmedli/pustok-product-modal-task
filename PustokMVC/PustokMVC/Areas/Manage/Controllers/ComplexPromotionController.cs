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
    public class ComplexPromotionController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _environment;

        public ComplexPromotionController(AppDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        public async Task<IActionResult> Index()
        {
            List<ComplexPromotion> complexPromotions = await _context.ComplexPromotions.Where(n => !n.IsDeleted).ToListAsync();

            if (complexPromotions is null)
            {
                return NotFound();
            }

            return View(complexPromotions);
        }

        public async Task<IActionResult> Details(int id)
        {
            ComplexPromotion complexPromotion = await _context.ComplexPromotions.Where(n => !n.IsDeleted && n.Id == id).FirstOrDefaultAsync();

            if (complexPromotion is null)
            {
                return NotFound();
            }

            return View(complexPromotion);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ComplexPromotion promotion)
        {
            if (promotion is null)
            {
                return NotFound();
            }

            if (promotion.Title == null || string.IsNullOrEmpty(promotion.Title.Trim()))
            {
                ModelState.AddModelError("Title", "This field cannot be an empty!");
                return View(promotion);
            }

            if (promotion.SubTitle == null || string.IsNullOrEmpty(promotion.SubTitle.Trim()))
            {
                ModelState.AddModelError("SubTitle", "This field cannot be an empty!");
                return View(promotion);
            }

            if (promotion.RedirectUrl == null || string.IsNullOrEmpty(promotion.RedirectUrl.Trim()))
            {
                ModelState.AddModelError("RedirectUrl", "This field cannot be an empty!");
                return View(promotion);
            }

            if (promotion.RedirectUrlText == null || string.IsNullOrEmpty(promotion.RedirectUrlText.Trim()))
            {
                ModelState.AddModelError("RedirectUrlText", "This field cannot be an empty!");
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
            await _context.ComplexPromotions.AddAsync(promotion);
            await _context.SaveChangesAsync();

            return RedirectToAction(controllerName: nameof(ComplexPromotion), actionName: nameof(Index));
        }

        public async Task<IActionResult> Update(int id)
        {
            ComplexPromotion complexPromotion = await _context.ComplexPromotions.Where(n => !n.IsDeleted && n.Id == id).FirstOrDefaultAsync();

            if (complexPromotion is null)
            {
                return NotFound();
            }

            return View(complexPromotion);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int id, ComplexPromotion promotion)
        {
            ComplexPromotion complexPromotion = await _context.ComplexPromotions.Where(n => !n.IsDeleted && n.Id == id).FirstOrDefaultAsync();

            if (complexPromotion is null)
            {
                return NotFound();
            }

            if (promotion is null)
            {
                return NotFound();
            }

            if (promotion.Title == null || string.IsNullOrEmpty(promotion.Title.Trim()))
            {
                ModelState.AddModelError("Title", "This field cannot be an empty!");
                return View(promotion);
            }

            if (promotion.SubTitle == null || string.IsNullOrEmpty(promotion.SubTitle.Trim()))
            {
                ModelState.AddModelError("SubTitle", "This field cannot be an empty!");
                return View(promotion);
            }

            if (promotion.RedirectUrl == null || string.IsNullOrEmpty(promotion.RedirectUrl.Trim()))
            {
                ModelState.AddModelError("RedirectUrl", "This field cannot be an empty!");
                return View(promotion);
            }

            if (promotion.RedirectUrlText == null || string.IsNullOrEmpty(promotion.RedirectUrlText.Trim()))
            {
                ModelState.AddModelError("RedirectUrlText", "This field cannot be an empty!");
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

                complexPromotion.Image = imageName;
            }

            complexPromotion.Title = promotion.Title;
            complexPromotion.SubTitle = promotion.SubTitle;
            complexPromotion.RedirectUrl = promotion.RedirectUrl;
            complexPromotion.RedirectUrlText = promotion.RedirectUrlText;
            complexPromotion.PromotionSize = promotion.PromotionSize;
            complexPromotion.UpdatedDate = DateTime.Now;
            _context.ComplexPromotions.Update(complexPromotion);
            await _context.SaveChangesAsync();

            return RedirectToAction(controllerName: nameof(ComplexPromotion), actionName: nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            ComplexPromotion promotion = await _context.ComplexPromotions.Where(n => !n.IsDeleted && n.Id == id).FirstOrDefaultAsync();

            if (promotion is null)
            {
                return NotFound();
            }

            return View(promotion);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(ComplexPromotion promotion)
        {
            ComplexPromotion dbPromotion = await _context.ComplexPromotions.Where(n => !n.IsDeleted && n.Id == promotion.Id).FirstOrDefaultAsync();

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
            _context.ComplexPromotions.Update(dbPromotion);
            await _context.SaveChangesAsync();
            return RedirectToAction(controllerName: nameof(ComplexPromotion), actionName: nameof(Index));
        }
    }
}
