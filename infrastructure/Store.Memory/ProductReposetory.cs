using System.Linq;

namespace Store.Memory
{
    //public class ProductReposetory : IProductRepository
    //{
    //    //private readonly Product[] products = new[]
    //    //{
    //    //    Product.Mapper.Map(Product.DtoFactory.Create(1, "Chees", "Podoksha", "Качотта", "Тут описание", 150m)),
    //    //    Product.Mapper.Map(Product.DtoFactory.Create(2, "Chees", "Podoksha", "Рикотта", "Тут описание", 80m)),
    //    //    Product.Mapper.Map(Product.DtoFactory.Create(3,"Chees", "Podoksha", "Качокавалла", "Тут описание", 190m)),
    //    //    Product.Mapper.Map(Product.DtoFactory.Create(4, "Chees", "Подносковье", "Адыгейский", "Тут описание", 100m)),
    //    //    Product.Mapper.Map(Product.DtoFactory.Create(5, "Chees", "Лебедева", "Маскарпоне", "Тут описание", 60m)),
    //    //    Product.Mapper.Map(Product.DtoFactory.Create(6, "Оборудование", "Podoksha", "Лира", "Тут описание", 1200m))
    //    //};


    //    //public Product[] GetAllByСategories(string query)
    //    //{
    //    //    return products
    //    //         .Where(chees => chees.Сategories.Contains(query))
    //    //         .ToArray();
    //    //}

    //    //public Product[] GetAllByTitleOrManufacture(string query)
    //    //{
    //    //    return products.Where(product => product.Manufacturer.Contains(query) ||
    //    //                                  product.Title.Contains(query))
    //    //                .ToArray();
    //    //}

    //    //public Product GetById(int id)
    //    //{
    //    //    return products.Single(product => product.Id == id);
    //    //}

    //    //public Product[] GetAllCategory(IEnumerable<int> productIds)
    //    //{
    //    //    var foundProducts = from product in products
    //    //                        join productId in productIds on product.Id equals productId
    //    //                        select product;

    //    //    return foundProducts.ToArray();
    //    //}
    //}
}