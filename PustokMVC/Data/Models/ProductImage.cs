using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models
{
    public class ProductImage : BaseEntity
    {
        public string Name { get; set; }
        public int ProductId { get; set; }
    }
}
