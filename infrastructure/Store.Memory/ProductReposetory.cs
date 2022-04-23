using System.Linq;

namespace Store.Memory
{
    public class ProductReposetory : IProductRepository
    {
        //Возможно нужно сделать список допустимых производителей
        private readonly Product[] products = new[]
        {
            new Product(1, "Качотта", "Chees", "Podoksha","Тут описание", 150m),
            new Product(2, "Рикотта", "Chees", "Podoksha","Тут описание", 80m),
            new Product(3, "Качокавалла", "Chees", "Podoksha","Тут описание", 190m),
            new Product(4, "Адыгейский", "Chees", "Подносковье","Тут описание", 80m),
            new Product(5, "Маскарпоне", "Chees", "Лебедева","Тут описание", 60m),
            new Product(6, "Лира", "Оборудование", "Podoksha","Тут описание", 1200m),
        };

        public Product[] GetAllByСategories(string query)
        {
           return products
                .Where(chees => chees.Сategories.Contains(query))
                .ToArray();
        }

        public Product[] GetAllByTitleOrManufacture(string query)
        {
            return products.Where(product => product.Manufacturer.Contains(query) || 
                                          product.Title.Contains(query))
                        .ToArray();
        }

        public Product GetById(int id)
        {
            return products.Single(product => product.Id == id);
        }

        public Product[] GetAllByIds(IEnumerable<int> productIds)
        {
            var foundProducts = from product in products
                                join productId in productIds on product.Id equals productId
                                select product;

            return foundProducts.ToArray();
        }
    }
}