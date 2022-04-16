using Microsoft.AspNetCore.Mvc;

namespace Store.Web.Controllers
{
    public class SearchController : Controller
    {
        private readonly ProductService productServise;

        public SearchController(ProductService productService)
        {
            this.productServise = productServise;
        }
        public IActionResult Index(string query)
        {
            var product = productServise.GetAllByQuery(query);
            return View(product);
        }
    }
}
