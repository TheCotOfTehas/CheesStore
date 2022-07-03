using Microsoft.AspNetCore.Mvc;
using Store.Web.App;

namespace Store.Web.Controllers
{
    public class ProductController : Controller
    {
        private readonly ProductService productService;

        public ProductController(ProductService productServise)
        {
            this.productService = productServise;
        }
        public async Task<IActionResult> Index(int id)
        {
            var product = await productService.GetByIdAsync(id);
            return View("Index", product);
        }
    }
}
 