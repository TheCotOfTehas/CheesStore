using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store
{
    public interface IProductRepository
    {
        Product[] GetAllByСategories(string categoryId);

        Product[] GetAllByTitleOrManufacture(string titlePart);

        Product GetById(int id);
        Product[] GetAllByIds(IEnumerable<int> productIds);
    }
}
