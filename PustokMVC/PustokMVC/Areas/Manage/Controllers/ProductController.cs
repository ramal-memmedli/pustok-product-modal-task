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
    public class ProductController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _environment;

        public ProductController(AppDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        public async Task<IActionResult> Index()
        {
            List<Product> products = await _context.Products.Where(n => !n.IsDeleted)
                                                            .Include(n => n.Genre)
                                                            .Include(n => n.ProductImages)
                                                            .ToListAsync();

            if (products is null)
            {
                return NotFound();
            }

            return View(products);
        }

        public async Task<IActionResult> Details(int id)
        {
            Product product = await _context.Products.Where(n => !n.IsDeleted && n.Id == id)
                                                            .Include(n => n.Genre)
                                                            .Include(n => n.Author)
                                                            .Include(n => n.ProductImages)
                                                            .FirstOrDefaultAsync();

            if (product is null)
            {
                return NotFound();
            }

            return View(product);
        }

        public async Task<IActionResult> Create()
        {
            ViewData["Genres"] = await _context.Genres.Where(n => !n.IsDeleted).ToListAsync();
            ViewData["Authors"] = await _context.Authors.Where(n => !n.IsDeleted).ToListAsync();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Product product)
        {
            ViewData["Genres"] = await _context.Genres.Where(n => !n.IsDeleted).ToListAsync();
            ViewData["Authors"] = await _context.Authors.Where(n => !n.IsDeleted).ToListAsync();

            if (product is null)
            {
                return NotFound();
            }

            if (product.Name is null)
            {
                ModelState.AddModelError("Name", "This field cannot be empty!");
                return View(product);
            }

            if (double.IsNaN(product.SalePrice))
            {
                ModelState.AddModelError("SalePrice", "Sale price must be number!");
                return View(product);
            }

            if (double.IsNaN(product.CostPrice))
            {
                ModelState.AddModelError("CostPrice", "Cost price must be number!");
                return View(product);
            }

            if (double.IsNaN(product.DiscountPrice))
            {
                ModelState.AddModelError("DiscountPrice", "Discount price must be number!");
                return View(product);
            }

            if (product.Description is null)
            {
                ModelState.AddModelError("Description", "This field cannot be empty!");
                return View(product);
            }

            if (product.InfoText is null)
            {
                ModelState.AddModelError("InfoText", "This field cannot be empty!");
                return View(product);
            }

            if (product.AuthorId < 1)
            {
                ModelState.AddModelError("AuthorId", "This field cannot be empty!");
                return View(product);
            }

            if (product.GenreId < 1)
            {
                ModelState.AddModelError("GenreId", "This field cannot be empty!");
                return View(product);
            }

            if (product.ImageFile is null)
            {
                ModelState.AddModelError("ImageFile", "This field cannot be empty!");
                return View(product);
            }


            decimal size = (decimal)product.ImageFile.Length / 1024 / 1024;

            if (size > 3)
            {
                ModelState.AddModelError("ImageFile", "Max allowed image size is 3MB!");
                return View(product);
            }

            if (!product.ImageFile.ContentType.Contains("image/"))
            {
                ModelState.AddModelError("ImageFile", "Other file types except image not allowed!");
                return View(product);
            }

            string imageName = product.ImageFile.FileName;

            if (imageName.Length > 218)
            {
                imageName = imageName.Substring(imageName.Length - 218, 218);
            }

            imageName = Guid.NewGuid().ToString() + imageName;

            string path = Path.Combine(_environment.WebRootPath, "assets", "uploads", "productImages", imageName);

            using (FileStream fileStream = new FileStream(path, FileMode.Create))
            {
                await product.ImageFile.CopyToAsync(fileStream);
            }

            product.CreatedDate = DateTime.Now;
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();

            ProductImage productImage = new ProductImage();
            productImage.Name = imageName;
            productImage.ProductId = product.Id;
            productImage.CreatedDate = DateTime.Now;

            await _context.ProductImages.AddAsync(productImage);
            await _context.SaveChangesAsync();

            return RedirectToAction(controllerName: nameof(Product), actionName: nameof(Details), routeValues: new { product.Id });
        }

        public async Task<IActionResult> Update(int id)
        {
            Product product = await _context.Products.Where(n => !n.IsDeleted && n.Id == id).FirstOrDefaultAsync();

            ViewData["Genres"] = await _context.Genres.Where(n => !n.IsDeleted).ToListAsync();
            ViewData["Authors"] = await _context.Authors.Where(n => !n.IsDeleted).ToListAsync();

            if (product is null)
            {
                return NotFound();
            }

            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(Product product)
        {
            Product dbProduct = await _context.Products.Where(n => !n.IsDeleted && n.Id == product.Id).FirstOrDefaultAsync();

            ViewData["Genres"] = await _context.Genres.Where(n => !n.IsDeleted).ToListAsync();
            ViewData["Authors"] = await _context.Authors.Where(n => !n.IsDeleted).ToListAsync();

            if (product is null)
            {
                return NotFound();
            }

            if (product.Name is null)
            {
                ModelState.AddModelError("Name", "This field cannot be empty!");
                return View(product);
            }

            if (double.IsNaN(product.SalePrice))
            {
                ModelState.AddModelError("SalePrice", "Sale price must be number!");
                return View(product);
            }

            if (double.IsNaN(product.CostPrice))
            {
                ModelState.AddModelError("CostPrice", "Cost price must be number!");
                return View(product);
            }

            if (double.IsNaN(product.DiscountPrice))
            {
                ModelState.AddModelError("DiscountPrice", "Discount price must be number!");
                return View(product);
            }

            if (product.Description is null)
            {
                ModelState.AddModelError("Description", "This field cannot be empty!");
                return View(product);
            }

            if (product.InfoText is null)
            {
                ModelState.AddModelError("InfoText", "This field cannot be empty!");
                return View(product);
            }

            if (product.AuthorId < 1)
            {
                ModelState.AddModelError("AuthorId", "This field cannot be empty!");
                return View(product);
            }

            if (product.GenreId < 1)
            {
                ModelState.AddModelError("GenreId", "This field cannot be empty!");
                return View(product);
            }

            if (product.ImageFile is null)
            {
                ModelState.AddModelError("ImageFile", "This field cannot be empty!");
                return View(product);
            }

            decimal size = (decimal)product.ImageFile.Length / 1024 / 1024;

            if (size > 3)
            {
                ModelState.AddModelError("ImageFile", "Max allowed image size is 3MB!");
                return View(product);
            }

            if (!product.ImageFile.ContentType.Contains("image/"))
            {
                ModelState.AddModelError("ImageFile", "Other file types except image not allowed!");
                return View(product);
            }

            string imageName = product.ImageFile.FileName;

            if (imageName.Length > 218)
            {
                imageName = imageName.Substring(imageName.Length - 218, 218);
            }

            imageName = Guid.NewGuid().ToString() + imageName;

            string path = Path.Combine(_environment.WebRootPath, "assets", "uploads", "productImages", imageName);

            using (FileStream fileStream = new FileStream(path, FileMode.Create))
            {
                await product.ImageFile.CopyToAsync(fileStream);
            }

            dbProduct.UpdatedDate = DateTime.Now;
            dbProduct.Name = product.Name;
            dbProduct.SalePrice = product.SalePrice;
            dbProduct.CostPrice = product.CostPrice;
            dbProduct.DiscountPrice = product.DiscountPrice;
            dbProduct.Description = product.Description;
            dbProduct.InfoText = product.InfoText;
            dbProduct.IsNew = product.IsNew;
            dbProduct.IsFeatured = product.IsFeatured;
            dbProduct.GenreId = product.GenreId;
            dbProduct.AuthorId = product.AuthorId;

            _context.Products.Update(dbProduct);
            await _context.SaveChangesAsync();

            ProductImage dbImage = await _context.ProductImages.Where(n => n.ProductId == dbProduct.Id).FirstOrDefaultAsync();
            dbImage.IsDeleted = true;

            ProductImage productImage = new ProductImage();
            productImage.Name = imageName;
            productImage.ProductId = product.Id;
            productImage.CreatedDate = DateTime.Now;

            await _context.ProductImages.AddAsync(productImage);
            await _context.SaveChangesAsync();

            return RedirectToAction(controllerName: nameof(Product), actionName: nameof(Details), routeValues: new { product.Id });
        }

        public async Task<IActionResult> Delete(int id)
        {
            Product product = await _context.Products.Where(n => !n.IsDeleted && n.Id == id).FirstOrDefaultAsync();

            if (product is null)
            {
                return NotFound();
            }

            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Product product)
        {
            Product dbProduct = await _context.Products.Where(n => !n.IsDeleted && n.Id == product.Id).FirstOrDefaultAsync();

            if (dbProduct is null)
            {
                return NotFound();
            }

            if (product.Name != "CONFIRM")
            {
                ModelState.AddModelError("Name", "Confirmation input doesn't match. Try again.");
                return View(product);
            }

            dbProduct.IsDeleted = true;
            _context.Products.Update(dbProduct);
            await _context.SaveChangesAsync();

            return RedirectToAction(controllerName: nameof(Product), actionName: nameof(Index));
        }
    }
}
