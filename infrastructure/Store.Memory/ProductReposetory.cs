using System.Linq;

namespace Store.Memory
{
    public class ProductReposetory : IProductRepository
    {
        private readonly Products[] chees = new[]
        {
            new Products(1, "Качотта", "Chees", "Podoksha"),
            new Products(2, "Рикотта", "Chees", "Podoksha"),
            new Products(3, "Качокавалла", "Chees", "Podoksha"),
            new Products(3, "Адыгейский", "Chees", "Подносковье"),
        };

        public Products[] GetAllByTitleOrManufacture(string titlePart)
        {
           return chees
                .Where(chees => chees.Title.Contains(titlePart))
                .ToArray();
        }

        public Products[] GetAllByСategories(string query)
        {
            return chees.Where(product => product
                .Сategories.Contains(query)
                || product.Title.Contains(query))
                .ToArray();
        }
    }
}