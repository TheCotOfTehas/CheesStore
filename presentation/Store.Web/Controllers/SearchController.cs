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
        public async Task<IActionResult> Index(string query)
        {
            var product = await productServise.GetAllByQueryAsync(query); 
            return View("Index", product);
        }
    }
}
