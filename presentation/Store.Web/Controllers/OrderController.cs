﻿using Microsoft.AspNetCore.Mvc;
using Store.Contractors;
using Store.Web.Models;
using System.Linq;
using System.Text.RegularExpressions;

namespace Store.Web.Controllers
{
    public class OrderController : Controller
    {
        private readonly IProductRepository productRepository;
        private readonly IOrderRepositorycs orderRepositorycs;
        private readonly IEnumerable<IDeliveryService> deliveryServices;
        private readonly INotificationService notificationService;


        public OrderController(IProductRepository productRepository,
                              IOrderRepositorycs orderRepositorycs,
                              INotificationService notificationService,
                              IEnumerable<IDeliveryService> deliveryServices)
        {
            this.productRepository = productRepository;
            this.orderRepositorycs = orderRepositorycs;
            this.notificationService = notificationService;
            this.deliveryServices = deliveryServices;
        }

        [HttpPost]
        public ActionResult SendConfirmationCode(int id, string cellPhone)
        {
            var order = orderRepositorycs.GetById(id);
            var model = Map(order);
            if (!IsValidCellPhone(cellPhone))
            {
                model.Errors["cellPhone"] = "Номер телефона не соответствует";
                return View("Index", model);
            }

            int code = 1111;
            HttpContext.Session.SetInt32(cellPhone, code);
            notificationService.SendConfirmationCode(cellPhone, code);

            return View("Confirmation", 
                new ConfirmationModel 
                { 
                    OrderId = id,
                    CellPhone = cellPhone 
                });
        }

        private bool IsValidCellPhone(string cellPhone)
        {
            if (cellPhone == null)
                return false;

            cellPhone = cellPhone
                .Replace(" ", "")
                .Replace("-", "");

            return Regex.IsMatch(cellPhone, @"^\+?\d{11}$");   
        }

        [HttpGet]
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

            return RedirectToAction("Index", "Order");
        }

        private void SaveOrderAndCart(Order order, Cart cart)
        {
            orderRepositorycs.Update(order);

            cart.TotalCount = order.TotatalCount;
            cart.TotalPrice = order.TotalPrice;
            HttpContext.Session.Set(cart);
        }

        [HttpPost]
        public IActionResult RemoveItem(int productId)
        {

            (Order order, Cart cart) = GetOrderAndCart();

            order.RemoveItem(productId);

            SaveOrderAndCart(order, cart);

            return RedirectToAction("Index", "Order");
        }

        [HttpPost]
        public IActionResult Confirmate(int id, string cellPhone, int code)
        {
            int? storeCode = HttpContext.Session.GetInt32(cellPhone);
            if (storeCode == null)
                return View("Confirmation",
                new ConfirmationModel
                {
                    OrderId = id,
                    CellPhone = cellPhone,
                    Errors = new Dictionary<string, string>
                    {
                       { "code", "Пустой код, повторите отправку." }
                    },
                });

            if (storeCode != code)
            {
                return View("Confirmation",
                new ConfirmationModel
                {
                    OrderId = id,
                    CellPhone = cellPhone,
                    Errors = new Dictionary<string, string>
                    {
                       { "code", "Неверный код." }
                    },
                });
            }
            //todo : сохранить CellPhone
            HttpContext.Session.Remove(cellPhone);

            var model = new DeliveryModel
            {
                OrderId = id,
                Methods = deliveryServices.ToDictionary(service => service.UniqueCode,
                                                        servise => servise.Title)
            };

            return View("DeliveryMethod", model);
        }

        [HttpPost]
        public IActionResult StartDelivery(int id, string uniqueCode)
        {
            var deliverService = deliveryServices.Single(service => service.UniqueCode == uniqueCode);
            var order = orderRepositorycs.GetById(id);

            var form = deliverService.CreateForm(order);

            return View("DeliveryStep", form);
        }

        [HttpPost]
        public IActionResult NextDelivery(int id, string uniqueCode, int step, Dictionary<string, string> values)
        {
            var deliverService = deliveryServices.Single(service => service.UniqueCode == uniqueCode);

            var form = deliverService.MoveNext(id, step, values);

            if (form.IsFinal)
            {
                return null;
            }

            return View("DeliveryStep", form);
        }
    }
}
