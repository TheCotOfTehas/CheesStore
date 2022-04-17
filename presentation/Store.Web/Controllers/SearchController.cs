using Microsoft.AspNetCore.Mvc;

namespace Store.Web.Controllers
{
    public class SearchController : Controller
    {
        private readonly ProductService productServise;

        public SearchController(ProductService productServise)
        {
            this.productServise = productServise;
        }
        public IActionResult Index(string query)
        {
            var product = productServise.GetAllByQuery(query); //Object reference not set to an instance of an object."
            return View(product);
        }
    }
}
