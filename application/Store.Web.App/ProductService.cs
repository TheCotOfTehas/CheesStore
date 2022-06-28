
namespace Store.Web.App
{
    public class ProductService
    {
        private readonly IProductRepository productRepository;

        public ProductService(IProductRepository productRepository)
        {
            this.productRepository = productRepository;
        }

        public IReadOnlyCollection<ProductModel> GetAllByQuery(string query)
        {
            var products = Product.IsСategories(query)
                ? productRepository.GetAllByСategories(query)
                : productRepository.GetAllByTitleOrManufacture(query);

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
                Сategories = product.Сategories
            };
        }

        public ProductModel GetById(int id)
        {
            var product = productRepository.GetById(id);
            return Map(product);
        }
    }
}
