using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models
{
    public class Slider : BaseEntity
    {
        public string Title1 { get; set; }
        public string Title2 { get; set; }
        public string Description { get; set; }
        public string RedirectUrl { get; set; }
        public string RedirectUrlText { get; set; }
        public string Image { get; set; }
        public int Order { get; set; }
        [NotMapped]
        public IFormFile ImageFile { get; set; }
    }
}
