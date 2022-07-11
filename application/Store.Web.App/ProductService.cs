using static System.Net.Mime.MediaTypeNames;

namespace Store.Web.App
{
    public class ProductService
    {
        private readonly IProductRepository productRepository;

        public ProductService(IProductRepository productRepository)
        {
            this.productRepository = productRepository;
        }

        public async Task<IReadOnlyCollection<ProductModel>> GetAllByQueryAsync(string query)
        {
            Product[] products;
            if (Product.IsСategories(query))
            {
                products = await productRepository.GetAllByСategoriesAsync(query);
                if (products.Length == 0 || products == null)
                    products = await productRepository.GetAllByTitleOrManufactureAsync(query);
            }
            else
            {
                products = await productRepository.GetAllByTitleOrManufactureAsync(query);
            }



            return products.Select(Map)
                           .ToArray();
        }

        private ProductModel Map(Product product)
        {
            return new ProductModel
            {
                Id = product.Id,
                Title = product.Title,
                Description = product.Description,
                Manufacturer = product.Manufacturer,
                Price = product.Price,
                Сategories = product.Сategories,
                img = product.Img,
            };
        }

        public async Task<ProductModel> GetByIdAsync(int id)
        {
            var product = await productRepository.GetByIdAsync(id);
            return Map(product);
        }
    }
}
