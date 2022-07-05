
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
            var products = Product.IsСategories(query)
                ? await productRepository.GetAllByСategoriesAsync(query)
                : await productRepository.GetAllByTitleOrManufactureAsync(query);

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
