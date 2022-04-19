using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store
{
    public class ProductService
    {
        private readonly IProductRepository productRepository;

        public ProductService(IProductRepository productRepository)
        {
            this.productRepository = productRepository;
        }

        public Product[] GetAllByQuery(string query)
        {
            var categories = productRepository.GetAllByСategories(query);
            if (categories.Length != 0)
                return categories;
            else
                return productRepository.GetAllByTitleOrManufacture(query);
        }

        public object GetById(int id)
        {
            return productRepository.GetById(id);
        }
    }
}
