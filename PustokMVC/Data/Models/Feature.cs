using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models
{
    public class Feature : BaseEntity
    {
        public string Header { get; set; }
        public string Description { get; set; }
        public string Icon { get; set; }
    }
}
