using Data.DAL;
using Data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PustokMVC.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PustokMVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;

        public HomeController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            HomeVM homeVM = new HomeVM();

            List<Slider> sliders = await _context.Sliders.ToListAsync();

            homeVM.Sliders = _context.Sliders.Where(n => !n.IsDeleted).ToList();
            homeVM.Features = _context.Features.Where(n => !n.IsDeleted).ToList();
            homeVM.SimplePromotions = _context.SimplePromotions.Where(n => !n.IsDeleted).ToList();
            homeVM.ComplexPromotions = _context.ComplexPromotions.Where(n => !n.IsDeleted).ToList();
            homeVM.Genres = _context.Genres.Where(n => !n.IsDeleted).ToList();
            homeVM.FeaturedProducts = _context.Products.Where(n => !n.IsDeleted && n.IsFeatured).Include(n => n.Author).Take(10).ToList();
            homeVM.NewProducts = _context.Products.Where(n => !n.IsDeleted && n.IsNew).Include(n => n.Author).Take(10).ToList();
            homeVM.DiscountedProducts = _context.Products.Where(n => !n.IsDeleted && n.DiscountPrice > 0).Include(n => n.Author).Take(10).ToList();

            return View(homeVM);
        }

        public async Task<IActionResult> GetProduct(int id)
        {
            Product product = await _context.Products.Where(n => !n.IsDeleted && n.Id == id)
                                                     .Include(n => n.Genre)
                                                     .Include(n => n.Author)
                                                     .Include(n => n.ProductImages)
                                                     .FirstOrDefaultAsync();

            if(product == null)
            {
                return NotFound();
            }

            return PartialView("_ProductModalPartial", product);
        }
    }
}
