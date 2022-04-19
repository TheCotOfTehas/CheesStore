using Microsoft.AspNetCore.Mvc;

namespace Store.Web.Controllers
{
    public class ProductController : Controller
    {
        private readonly ProductService productServise;

        public ProductController(ProductService productServise)
        {
            this.productServise = productServise;
        }
        public IActionResult Index(int id)
        {
            var product = productServise.GetById(id);
            return View(product);
        }
    }
}
