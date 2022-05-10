using Microsoft.AspNetCore.Http;
using PhoneNumbers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Web.App
{
    public class OrderService
    {
        private readonly IProductRepository productRepository;
        private readonly IOrderRepository orderRepository;
        private readonly INotificationService notificationService;
        private readonly IHttpContextAccessor httpContextAccessor;

        protected ISession Session => httpContextAccessor.HttpContext.Session;

        public OrderService(IProductRepository productRepository,
                            IOrderRepository orderRepositorycs,
                            INotificationService notificationService,
                            IHttpContextAccessor httpContextAccessor)
        {
            this.productRepository = productRepository;
            this.orderRepository = orderRepositorycs;
            this.notificationService = notificationService;
            this.httpContextAccessor = httpContextAccessor;
        }

        public bool TryGetModel(out OrderModel model)
        {
            if (TryGetOrder(out var order))
            {
                model = Map(order);
                return true;
            }

            model = null;
            return false;
        }

        public bool TryGetOrder(out Order order)
        {
            if(Session.TryGetCart(out var cart))
            {
                order = orderRepository.GetById(cart.OrderId);
                return true;
            }

            order = null;
            return false;
        }

        internal OrderModel Map(Order order)
        {
            var products = GetProducts(order);
            var items = from item in order.Items
                        join product in products on item.ProductId equals product.Id
                        select new OrderItemModel
                        {
                            ProductId = product.Id,
                            Titel = product.Title,
                            Manufacturer = product.Manufacturer,
                            Price = product.Price,
                            Categore = product.Сategories,
                            Count = product.Count,
                        };

            return new OrderModel
            {
                Id = order.Id,
                Items = items.ToArray(),
                ToralCount = order.TotatalCount,
                TotalPrice = order.TotalPrice,
                CellPhone = order.CellPhone,
                DeliveryDescription = order.Delivery?.Description,
                PaymentDescription = order.Payment?.Description
            };
        }

        internal IEnumerable<Product> GetProducts(Order order)
        {
            var productId = order.Items.Select(x => x.ProductId);

            return productRepository.GetAllByIds(productId);
        }

        public OrderModel AppProduct(int productId, int count)
        {
            if (count < 1)
                throw new InvalidOperationException("Нельзя добавить менее одного товара");

            if (!TryGetOrder(out Order order))
                order = orderRepository.Create();
            
            AddOrUpdateProduct(order, productId, count);
            UpdateSession(order);
            

            return Map(order);         
        }

        internal void AddOrUpdateProduct(Order order, int productId, int count)
        {
            var product = productRepository.GetById(productId);
            if (order.Items.TryGet(productId, out OrderItem orderItem))
                orderItem.Count += count;
            else
                order.Items.Add(product.Id, product.Price, count);

            orderRepository.Update(order);
        }

        internal void UpdateSession(Order order)
        {
            var cart = new Cart(order.Id, order.TotatalCount, order.TotalPrice);
            Session.Set(cart);
        }

        public OrderModel UpdateProduct(int productId, int count)
        {
            var order = GetOrder();
            order.Items.Get(productId).Count = count;

            orderRepository.Update(order);
            UpdateSession(order);

            return Map(order);
        }

        public OrderModel RemoveProduct(int productId)
        {
            var order = GetOrder();
            order.Items.Remove(productId);

            orderRepository.Update(order);

            return Map(order);
        }

        public Order GetOrder()
        {
            if (TryGetOrder(out Order order))
                return order;

            throw new InvalidOperationException("Empty session");
        }

        public OrderModel SendConfirmation(string cellPhone)
        {
            var order = GetOrder();
            var model = Map(order);

            if (TryFormatPhone(cellPhone, out string formattedPhone))
            {
                var confirmmationCode = 1111; //todo: random.Next(1000,10000) =
                model.CellPhone = formattedPhone;
                Session.SetInt32(formattedPhone, confirmmationCode);
                notificationService.SendConfirmationCode(formattedPhone, confirmmationCode);
            }
            else
                model.Errors["cellPhone"] = "Номер телефона не соответствует";

            return model;
        }

        private readonly PhoneNumberUtil phoneNumberUtil = PhoneNumberUtil.GetInstance();

        internal bool TryFormatPhone(string cellPhone, out string formattedPhone)
        {
            try
            {
                var phoneNumber = phoneNumberUtil.Parse(cellPhone, "ru");
                formattedPhone = phoneNumberUtil.Format(phoneNumber, PhoneNumberFormat.INTERNATIONAL);
                return true;
            }
            catch (NumberParseException)
            {
                formattedPhone = null;
                return false;
            }
        }

        public OrderModel ConfirmCellPhone(string cellPhone, int confirmationCode)
        {
            int? storedCode = Session.GetInt32(cellPhone);
            var model = new OrderModel();

            if(storedCode == null)
            {
                model.Errors["cellPhone"] = "Произощла ошибка. Введите новый код";
                return model;
            }

            if(storedCode != confirmationCode)
            {
                model.Errors["confirmationCode"] = "Не правильный когд. Придётся ввести заново";
                return model;
            }

            var order = GetOrder();
            order.CellPhone = cellPhone;
            orderRepository.Update(order);

            Session.Remove(cellPhone);

            return Map(order);
        }

        public OrderModel SetDelivery(OrderDelivery delivery)
        {
            var order = GetOrder();
            order.Delivery = delivery;
            orderRepository.Update(order);

            return Map(order);
        }

        public OrderModel SetPayment(OrderPayment payment)
        {
            var order = GetOrder();
            order.Payment = payment;
            orderRepository.Update(order);

            return Map(order);
        }
    }
}
