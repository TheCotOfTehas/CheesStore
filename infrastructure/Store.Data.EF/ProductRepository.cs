using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Data.EF
{
    public class ProductRepository : IProductRepository
    {
        private readonly DbContextFactory dbContextFactory;
        public ProductRepository(DbContextFactory dbContextFactory)
        {
            this.dbContextFactory = dbContextFactory;
        }
        public Product[] GetAllByСategories(string query)  
        {
            var context = dbContextFactory.Create(typeof(ProductRepository));

            var productsDto = context
                .Products
                .Where(chees => chees.Сategories == query)
                .ToArray();

            return productsDto.Select(Product.Mapper.Map).ToArray();
        }

        public Product[] GetAllCategory(IEnumerable<int> productIds)
        {
            var context = dbContextFactory.Create(typeof(ProductRepository));
            return context
                .Products
                .Where(product => productIds.Contains(product.Id))
                .Select(Product.Mapper.Map)
                .ToArray();
        }

        public Product[] GetAllByTitleOrManufacture(string query)
        {
            var context = dbContextFactory.Create(typeof(ProductRepository));
            return context
                .Products
                .Select(Product.Mapper.Map)
                .Where(product => product.Manufacturer.Contains(query) ||
                                          product.Title.Contains(query))
                .ToArray();
        }

        public Product GetById(int id)
        {
            var context = dbContextFactory.Create(typeof(ProductRepository));
            return context
                .Products
                .Select(Product.Mapper.Map)
                .Single(product => product.Id == id);
        }
    }
}
