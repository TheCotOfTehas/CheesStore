using Microsoft.AspNetCore.Mvc;
using Store.Web.Models;

namespace Store.Web.Controllers
{
    public class CartController : Controller
    {
        private readonly IProductRepository productRepository;
        private readonly IOrderRepositorycs orderRepositorycs;

        public CartController(IProductRepository productRepository,
                              IOrderRepositorycs orderRepositorycs)
        {
            this.productRepository = productRepository;
            this.orderRepositorycs = orderRepositorycs;
        }
        public IActionResult Add(int id)
        {
            Order order;
            Cart cart;
            if(HttpContext.Session.TryGetCart(out cart))
            {
                order = orderRepositorycs.GetById(cart.OrderId);
            }
            else
            {
                order = orderRepositorycs.Create();
                cart = new Cart(order.Id);
            }

            var product = productRepository.GetById(id);
            order.AddItem(product, 1);
            orderRepositorycs.Update(order);

            cart.TotalCount = order.TotatalCount;
            cart.TotalPrice = order.TotalPrice;

            HttpContext.Session.Set(cart);

            return RedirectToAction("Index", "Product", new { id });
        }
    }
}
