using Microsoft.AspNetCore.Mvc;
using Store.Contractors;
using Store.Web.App;
using Store.Web.Contractors;

namespace Store.Web.Controllers
{
    public class OrderController : Controller
    {
        private readonly OrderService orderService;
        private readonly IEnumerable<IDeliveryService> deliveryServices;
        private readonly IEnumerable<IPaymentService> paymentServices;
        private readonly IEnumerable<IWebContractorService> webContractorServices;

        public OrderController(OrderService orderService,
                              IEnumerable<IDeliveryService> deliveryServices,
                              IEnumerable<IPaymentService> paymentServices,
                              IEnumerable<IWebContractorService> webContractorServices
        )
        {
            this.orderService = orderService;
            this.deliveryServices = deliveryServices;
            this.webContractorServices = webContractorServices;
            this.paymentServices = paymentServices;
        }

        [HttpPost]
        public async Task<IActionResult> SendConfirmationCode(int id, string cellPhone)
        {
            OrderModel model = await orderService.SendConfirmationAsync(cellPhone);

            return View("Confirmation", model);
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var (hasValue, model) = await orderService.TryGetModelAsync();
            if (hasValue)
                return View(model);

            return View("Empty");
        }

        [HttpPost]
        public async Task<IActionResult> AddItem(int productId, int count = 1)
        {
            await orderService.AppProductAsync(productId, count);

            return RedirectToAction("Index", "Product", new {id = productId});
        }

        [HttpPost]
        public async Task<IActionResult> UpdateItem(int productId, int count)
        {
            var model = await orderService.UpdateProductAsync(productId, count);

            return View("Index", model);
        }

        [HttpPost]
        public async Task<IActionResult> RemoveItem(int productId)
        {
            var model = await orderService.RemoveProductAsync(productId);

            return View("Index", model);
        }
















        [HttpPost]
        public async Task<IActionResult> ConfirmateCellPhone(string cellPhone, int confirmationCode)
        {
            var model = await orderService.ConfirmCellPhoneAsync(cellPhone, confirmationCode);

            if(model.Errors.Count > 0)
                return View("Confirmation", model);


            var deliveryMethod = deliveryServices.ToDictionary(service => service.Name,
                                                               servise => servise.Title);

            return View("DeliveryMethod", deliveryMethod);
        }

        [HttpPost]
        public async Task<IActionResult> StartDelivery(string serviceName)
        {
            var deliveryMethod = deliveryServices.Single(service => service.Name == serviceName);
            var order = await orderService.GetOrderAsync();
            var form = deliveryMethod.FirstForm(order);

            var webContractorService = webContractorServices.SingleOrDefault(service => service.Name == serviceName);//??
            if(webContractorService == null)
                return View("DeliveryStep", form);

            var returnUri = GetReturnUri(nameof(NextDelivery));
            var redirectUri = await webContractorService.StartSessionAsync(form.Parameters, returnUri);

            return Redirect(redirectUri.ToString());
        }

        private Uri GetReturnUri(string action)
        {
            var builder = new UriBuilder(Request.Scheme, Request.Host.Host)
            {
                Path = Url.Action(action),
                Query = null,
            };

            if (Request.Host.Port != null)
                builder.Port = Request.Host.Port.Value;

            return builder.Uri;
        }

        [HttpPost]
        public async Task<IActionResult> NextDelivery(string serviceName, int step, Dictionary<string, string> values)
        {
            var deliverService = deliveryServices.Single(service => service.Name == serviceName);

            var form = deliverService.NextForm(step, values);

            if(!form.IsFinal)
                return View("DeliveryStep", form);

            var delivery = deliverService.GetDelivery(form);
            await orderService.SetDeliveryAsync(delivery);

            var paymantMethods = paymentServices.ToDictionary(service => service.Name,
                                                                         servise => servise.Title);

            return View("PaymentMethod", paymantMethods);
        }

        [HttpPost]
        public async Task<IActionResult> StartPayment(string serviceName, int step, Dictionary<string, string> values)
        {
            var paymentService = paymentServices.Single(service => service.Name == serviceName);
            var order = await orderService.GetOrderAsync();
            var form = paymentService.FirstForm(order);

            var webContractorService = webContractorServices.SingleOrDefault(service => service.Name == serviceName);
            if (webContractorService == null)
                return View("PaymentStep", form);

            var returnUri = GetReturnUri(nameof(NextPayment));
            var redirectUri = await webContractorService.StartSessionAsync(form.Parameters, returnUri);
            //var payment = paymentService.GetPayment(form);
            //var model = orderService.SetPaymentAsync(payment);

            //return View("Finish", model);
            return Redirect(redirectUri.ToString());
        }

        [HttpPost]
        public async Task<IActionResult> NextPayment(string serviceName, int step, Dictionary<string, string> values)
        {
            var paymentService = paymentServices.Single(service => service.Name == serviceName);

            var form = paymentService.NextForm(step, values);
            if (!form.IsFinal)
                return View("PaymentStep", form);

            var payment = paymentService.GetPayment(form);
            var model = await orderService.SetPaymentAsync(payment);

            return View("Finish", model);
        }
    }
}
