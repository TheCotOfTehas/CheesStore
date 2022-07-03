using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store
{
    public interface IProductRepository
    {
        Task<Product[]> GetAllByСategoriesAsync(string categoryId);

        Task<Product[]> GetAllByTitleOrManufactureAsync(string titlePart);

        Task<Product> GetByIdAsync(int id);

        Task<Product[]> GetAllCategoryAsync(IEnumerable<int> productIds);
    }
}
