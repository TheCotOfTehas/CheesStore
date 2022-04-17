using System.Linq;

namespace Store.Memory
{
    public class ProductReposetory : IProductRepository
    {
        //Возможно нужно сделать список допустимых производителей
        private readonly Products[] chees = new[]
        {
            new Products(1, "Качотта", "Chees", "Podoksha"),
            new Products(2, "Рикотта", "Chees", "Podoksha"),
            new Products(3, "Качокавалла", "Chees", "Podoksha"),
            new Products(4, "Адыгейский", "Chees", "Подносковье"),
            new Products(5, "Маскарпоне", "Chees", "Лебедева"),
            new Products(6, "Лира", "Оборудование", "Podoksha"),
        };

        public Products[] GetAllByСategories(string query)
        {
           return chees
                .Where(chees => chees.Сategories.Contains(query))
                .ToArray();
        }

        public Products[] GetAllByTitleOrManufacture(string query)
        {
            return chees.Where(product => product.Manufacturer.Contains(query) || 
                                          product.Title.Contains(query))
                        .ToArray();
        }
    }
}