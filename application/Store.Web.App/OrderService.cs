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

        public async Task<(bool hasValue, OrderModel model)> TryGetModelAsync()
        {
            var (hasValue, order) = await TryGetOrderAsync();
            if (hasValue)
                return (true , await  MapAsync(order));

           return (false, null);
        }

        internal async Task<(bool hasValue, Order order)> TryGetOrderAsync()
        {
            if(Session.TryGetCart(out var cart))
            {
                var order = await orderRepository.GetByIdAsync(cart.OrderId);
                return (true, order);
            }

            return (false, null);
        }

        internal async Task<OrderModel> MapAsync(Order order)
        {
            var products = await GetProductsAsync(order);
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

        internal async Task<IEnumerable<Product>> GetProductsAsync(Order order)
        {
            var productId = order.Items.Select(x => x.ProductId);

            return await productRepository.GetAllCategoryAsync(productId);
        }

        public async Task<OrderModel> AppProductAsync(int productId, int count)
        {
            if (count < 1)
                throw new InvalidOperationException("Нельзя добавить менее одного товара");

            var (hasValue, order) = await TryGetOrderAsync();

            if (!hasValue)
                order = await orderRepository.CreateAsync();

            await AddOrUpdateProductAsync(order, productId, count);
            UpdateSession(order);
            

            return await MapAsync(order);         
        }

        internal async Task AddOrUpdateProductAsync(Order order, int productId, int count)
        {
            var product = await productRepository.GetByIdAsync(productId);

            if (order.Items.TryGet(productId, out OrderItem orderItem))
                orderItem.Count += count;
            else
                order.Items.Add(product.Id, product.Price, count);

            await orderRepository.UpdateAsync(order);
        }

        internal void UpdateSession(Order order)
        {
            var cart = new Cart(order.Id, order.TotatalCount, order.TotalPrice);
            Session.Set(cart);
        }

        public async Task<OrderModel> UpdateProductAsync(int productId, int count)
        {
            var order = await GetOrderAsync();
            order.Items.Get(productId).Count = count;

            await orderRepository.UpdateAsync(order);
            UpdateSession(order);

            return await MapAsync(order);
        }

        public async Task<OrderModel> RemoveProductAsync(int productId)
        {
            var order = await GetOrderAsync();
            order.Items.Remove(productId);

            await orderRepository.UpdateAsync(order);

            return await MapAsync(order);
        }

        public async Task<Order> GetOrderAsync()
        {
            var (hasValue, order) = await TryGetOrderAsync();
            if (hasValue)
                return order;

            throw new InvalidOperationException("Empty session");
        }

        public async Task<OrderModel> SendConfirmationAsync(string cellPhone)
        {
            var order = await GetOrderAsync();
            var model = await MapAsync(order);

            if (TryFormatPhone(cellPhone, out string formattedPhone))
            {
                var confirmmationCode = 1111;
                model.CellPhone = formattedPhone;
                Session.SetInt32(formattedPhone, confirmmationCode);
                await notificationService.SendConfirmationCodeAsync(formattedPhone, confirmmationCode);
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

        public async Task<OrderModel> ConfirmCellPhoneAsync(string cellPhone, int confirmationCode)
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

            var order = await GetOrderAsync();
            order.CellPhone = cellPhone;
            await orderRepository.UpdateAsync(order);

            Session.Remove(cellPhone);

            return await MapAsync(order);
        }

        public async Task<OrderModel> SetDeliveryAsync(OrderDelivery delivery)
        {
            var order = await GetOrderAsync();
            order.Delivery = delivery;
            await orderRepository.UpdateAsync(order);

            return await MapAsync(order);
        }

        public async Task<OrderModel> SetPaymentAsync(OrderPayment payment)
        {
            var order = await GetOrderAsync();
            order.Payment = payment;
            await orderRepository.UpdateAsync(order);
            Session.RemoveCart();

            await notificationService.StartProcessAsync(order);
            return await MapAsync(order);
        }
    }
}
