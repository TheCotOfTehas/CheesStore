using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Data
{
    public class ProductDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Manufacturer { get; set; }
        public string Сategories { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
    }
}
