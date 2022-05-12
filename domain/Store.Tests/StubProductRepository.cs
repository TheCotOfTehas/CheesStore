using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Tests
{
    class StubProductRepository : IProductRepository
    {
        public Product[] ResultOfGetAllByCategory { get; set; }
        public Product[] ResultOfGetByTitleOrManufaktyr { get; set; }
        public Product[] GetAllCategory(IEnumerable<int> productIds)
        {
            throw new NotImplementedException();
        }

        public Product[] GetAllByTitleOrManufacture(string titlePart)
        {
            return ResultOfGetByTitleOrManufaktyr;
        }

        public Product[] GetAllByСategories(string categoryId)
        {
            return ResultOfGetAllByCategory;
        }

        public Product GetById(int id)
        {
            throw new NotImplementedException();
        }
    }
}
