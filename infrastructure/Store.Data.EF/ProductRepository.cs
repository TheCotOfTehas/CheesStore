using Microsoft.EntityFrameworkCore;
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
        public async Task<Product[]> GetAllByСategoriesAsync(string query)  
        {
            var context = dbContextFactory.Create(typeof(ProductRepository));

            var dtos = await context.Products
                .Where(chees => chees.Сategories == query)
                .ToArrayAsync();


            return dtos.Select(Product.Mapper.Map)
                           .ToArray();
        }

        public async Task<Product[]> GetAllCategoryAsync(IEnumerable<int> bookIds)
        {
            var dbContext = dbContextFactory.Create(typeof(ProductRepository));

            var dtos = await dbContext.Products
                                      .Where(book => bookIds.Contains(book.Id))
                                      .ToArrayAsync();

            return dtos.Select(Product.Mapper.Map)
                       .ToArray();
        }


        public async Task<Product[]> GetAllByTitleOrManufactureAsync(string query)
        {
            if (string.IsNullOrWhiteSpace(query))  
                return Array.Empty<Product>();

            var context = dbContextFactory.Create(typeof(ProductRepository));


            var dtos = await context.Products
                .Where(product => product.Manufacturer.Contains(query) ||
                    product.Title.Contains(query))
                .ToArrayAsync();

            return dtos.Select(Product.Mapper.Map)
                .ToArray();
        }

        public async Task<Product> GetByIdAsync(int id)
        {
            var context = dbContextFactory.Create(typeof(ProductRepository));

            var dtos = await context.Products
                .SingleAsync(product => product.Id == id);

            return Product.Mapper.Map(dtos);
        }
    }
}
