using System.Linq;

namespace Store.Memory
{
    public class ProductReposetory : IProductRepository
    {
        private readonly Chees[] chees = new[]
        {
            new Chees(1, "Качотта"),
            new Chees(2, "Рикотта"),
            new Chees(3, "Качокавалла"),
        };

        public Chees[] GetByTitle(string titlePart)
        {
           return chees
                .Where(chees => chees.Title.Contains(titlePart))
                .ToArray();
        }
    }
}