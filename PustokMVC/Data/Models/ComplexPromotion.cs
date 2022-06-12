using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models
{
    public class ComplexPromotion : BaseEntity
    {
        public string Title { get; set; }
        public string SubTitle { get; set; }
        public string RedirectUrl { get; set; }
        public string RedirectUrlText { get; set; }
        public string Image { get; set; }
        public string PromotionSize { get; set; }

        [NotMapped]
        public IFormFile ImageFile { get; set; }
    }
}
