using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models
{
    public class Author : BaseEntity
    {
        public string FullName { get; set; }
        public List<Product> Products { get; set; }
    }
}
