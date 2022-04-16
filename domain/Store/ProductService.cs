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

        public Products[] GetAllByQuery(string query)
        {
            if (Products.IsСategories(query))
            {
                return productRepository.GetAllByСategories(query);
            }

            return productRepository.GetAllByTitleOrManufacture(query);
        } 
    }
}
