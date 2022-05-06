using Microsoft.AspNetCore.Mvc;
using Store.Web.App;

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
            var product = productServise.GetAllByQuery(query); 
            return View("Index", product);
        }
    }
}
