using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models
{
    public class Product : BaseEntity
    {
        public string Name { get; set; }
        public double SalePrice { get; set; }
        public double CostPrice { get; set; }
        public double DiscountPrice { get; set; }
        public string Description { get; set; }
        public string InfoText { get; set; }
        public bool IsNew { get; set; }
        public bool IsFeatured { get; set; }
        public int GenreId { get; set; }
        public Genre Genre { get; set; }
        public int AuthorId { get; set; }
        public Author Author { get; set; }
        public List<ProductImage> ProductImages { get; set; }
        [NotMapped]
        public IFormFile ImageFile { get; set; }
    }
}
