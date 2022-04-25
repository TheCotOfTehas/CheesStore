using Microsoft.AspNetCore.Mvc;
using Store.Web.Models;
using System.Linq;

namespace Store.Web.Controllers
{
    public class OrderController : Controller
    {
        private readonly IProductRepository productRepository;
        private readonly IOrderRepositorycs orderRepositorycs;

        public OrderController(IProductRepository productRepository,
                              IOrderRepositorycs orderRepositorycs)
        {
            this.productRepository = productRepository;
            this.orderRepositorycs = orderRepositorycs;
        }

        public IActionResult Index()
        {
            if (HttpContext.Session.TryGetCart(out Cart cart))
            {
                var order = orderRepositorycs.GetById(cart.OrderId);
                OrderModel model = Map(order);
                return View(model);
            }

            return View("Empty");
        }

        private OrderModel Map(Order order)
        {
            var productIds = order.Items.Select(item => item.ProductId);
            var products = productRepository.GetAllByIds(productIds);
            var itemModels = from item in order.Items
                             join product in products on item.ProductId equals product.Id
                             select new OrderItemModel
                             {
                                 ProductId = product.Id,
                                 Titel = product.Title,
                                 Manufacturer = product.Manufacturer,
                                 Price = item.Price,
                                 Count = item.Count,
                             };
            return new OrderModel
            {
                Id = order.Id,
                Items = itemModels.ToArray(),
                ToralCount = order.TotatalCount,
                TotalPrice = order.TotalPrice,
            };

        }
        [HttpPost]
        public IActionResult AddItem(int productId, int count = 1)
        {
            (Order order, Cart cart) = GetOrderAndCart();

            var product = productRepository.GetById(productId);
            order.AddOrUpdateItem(product, count);

            SaveOrderAndCart(order, cart);

            return RedirectToAction("Index", "Product", new {id = productId });
        }


        private (Order order, Cart cart) GetOrderAndCart()
        {
            Order order;
            if (HttpContext.Session.TryGetCart(out Cart cart))
            {
                order = orderRepositorycs.GetById(cart.OrderId);
            }
            else
            {
                order = orderRepositorycs.Create();
                cart = new Cart(order.Id);
            }

            return (order, cart);
        }

        [HttpPost]
        public IActionResult UpdateItem(int productId, int count)
        {
            (Order order, Cart cart) = GetOrderAndCart();

            order.GetItem(productId).Count = count;

            SaveOrderAndCart(order, cart);

            return RedirectToAction("Index", "Product", new { productId });
        }

        private void SaveOrderAndCart(Order order, Cart cart)
        {
            orderRepositorycs.Update(order);

            cart.TotalCount = order.TotatalCount;
            cart.TotalPrice = order.TotalPrice;
            HttpContext.Session.Set(cart);
        }

        public IActionResult RemoveItem(int id)
        {

            (Order order, Cart cart) = GetOrderAndCart();

            order.RemoveItem(id);

            SaveOrderAndCart(order, cart);

            return RedirectToAction("Index", "Product", new { id });
        }
    }
}
