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
    public class SliderController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _environment;

        public SliderController(AppDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        public async Task<IActionResult> Index()
        {
            List<Slider> sliders = await _context.Sliders.Where(n => !n.IsDeleted).ToListAsync();

            if(sliders is null)
            {
                return NotFound();
            }

            return View(sliders);
        }


        public async Task<IActionResult> Details(int id)
        {
            Slider slider = await _context.Sliders.Where(n => !n.IsDeleted && n.Id == id).FirstOrDefaultAsync();

            if (slider is null)
            {
                return NotFound();
            }

            return View(slider);
        }

        public async Task<IActionResult> Update(int id)
        {
            Slider slider = await _context.Sliders.Where(n => !n.IsDeleted && n.Id == id).FirstOrDefaultAsync();

            if (slider is null)
            {
                return NotFound();
            }

            return View(slider);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int id, Slider slider)
        {
            Slider dbSlider = await _context.Sliders.Where(n => !n.IsDeleted && n.Id == id).FirstOrDefaultAsync();

            if(dbSlider is null)
            {
                return NotFound();
            }

            if (slider.Title1 != null)
            {
                dbSlider.Title1 = slider.Title1;
            }

            if (slider.Title2 != null)
            {
                dbSlider.Title2 = slider.Title2;
            }

            if (slider.Description != null)
            {
                dbSlider.Description = slider.Description;
            }

            if (slider.RedirectUrl != null)
            {
                dbSlider.RedirectUrl = slider.RedirectUrl;
            }

            if (slider.RedirectUrlText != null)
            {
                dbSlider.RedirectUrlText = slider.RedirectUrlText;
            }

            if (slider.Order != 0)
            {
                dbSlider.Order = slider.Order;
            }

            if(slider.ImageFile != null)
            {
                decimal size = (decimal)slider.ImageFile.Length / 1024 / 1024;

                if (size > 3)
                {
                    ModelState.AddModelError("ImageFile", "Max allowed image size is 3MB!");
                    return View(slider);
                }

                if (!slider.ImageFile.ContentType.Contains("image/"))
                {
                    ModelState.AddModelError("ImageFile", "Other file types except image not allowed!");
                    return View(slider);
                }

                string imageName = slider.ImageFile.FileName;

                if (imageName.Length > 218)
                {
                    imageName = imageName.Substring(imageName.Length - 218, 218);
                }

                imageName = Guid.NewGuid().ToString() + imageName;

                string path = Path.Combine(_environment.WebRootPath, "assets", "uploads", "sliderImages", imageName);

                using (FileStream fileStream = new FileStream(path, FileMode.Create))
                {
                    await slider.ImageFile.CopyToAsync(fileStream);
                }

                dbSlider.Image = imageName;
            }

            if (slider != null)
            {
                dbSlider.UpdatedDate = DateTime.Now;

                _context.Sliders.Update(dbSlider);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(controllerName: nameof(Slider), actionName: nameof(Details), routeValues: new {id});
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Slider slider) {
            if (slider is null)
            {
                return NotFound();
            }

            if (slider.Title1 is null)
            {
                ModelState.AddModelError("Title1", "This field cannot be empty!");
                return View(slider);
            }

            if (slider.Title2 is null)
            {
                ModelState.AddModelError("Title2", "This field cannot be empty!");
                return View(slider);
            }

            if (slider.Description is null)
            {
                ModelState.AddModelError("Description", "This field cannot be empty!");
                return View(slider);
            }

            if (slider.RedirectUrl is null)
            {
                ModelState.AddModelError("RedirectUrl", "This field cannot be empty!");
                return View(slider);
            }

            if (slider.RedirectUrlText is null)
            {
                ModelState.AddModelError("RedirectUrlText", "This field cannot be empty!");
                return View(slider);
            }

            if (slider.ImageFile is null)
            {
                ModelState.AddModelError("ImageFile", "This field cannot be empty!");
                return View(slider);
            }

            if (slider.Order == 0)
            {
                ModelState.AddModelError("Order", "This field cannot be empty!");
                return View(slider);
            }

            decimal size = (decimal)slider.ImageFile.Length / 1024 / 1024;

            if (size > 3)
            {
                ModelState.AddModelError("ImageFile", "Max allowed image size is 3MB!");
                return View(slider);
            }

            if (!slider.ImageFile.ContentType.Contains("image/"))
            {
                ModelState.AddModelError("ImageFile", "Other file types except image not allowed!");
                return View(slider);
            }

            string imageName = slider.ImageFile.FileName;

            if (imageName.Length > 218)
            {
                imageName = imageName.Substring(imageName.Length - 218, 218);
            }

            imageName = Guid.NewGuid().ToString() + imageName;

            string path = Path.Combine(_environment.WebRootPath, "assets", "uploads", "sliderImages", imageName);

            using (FileStream fileStream = new FileStream(path, FileMode.Create))
            {
                await slider.ImageFile.CopyToAsync(fileStream);
            }

            slider.CreatedDate = DateTime.Now;
            slider.Image = imageName;

            await _context.Sliders.AddAsync(slider);
            await _context.SaveChangesAsync();

            return RedirectToAction(controllerName: nameof(slider), actionName: nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            Slider slider = await _context.Sliders.Where(n => !n.IsDeleted && n.Id == id).FirstOrDefaultAsync();

            if(slider is null)
            {
                return NotFound();
            }

            return View(slider);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Slider slider)
        {
            Slider dbSlider = await _context.Sliders.Where(n => !n.IsDeleted && n.Id == slider.Id).FirstOrDefaultAsync();

            if(dbSlider is null)
            {
                return NotFound();
            }

            if (slider is null)
            {
                return NotFound();
            }

            if (slider.Title1 != "CONFIRM")
            {
                ModelState.AddModelError("Title1", "Confirmation input doesn't match. Try again.");
                return View(slider);
            }

            dbSlider.IsDeleted = true;
            _context.Sliders.Update(dbSlider);
            await _context.SaveChangesAsync();
            return RedirectToAction(controllerName: nameof(Slider), actionName: nameof(Index));
        }
    }
}
