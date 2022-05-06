using Microsoft.AspNetCore.Mvc;
using Store.Contractors;
using Store.Memory;
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
        public IActionResult SendConfirmationCode(int id, string cellPhone)
        {
            var model = orderService.SendConfirmation(cellPhone);

            return View("Confirmation", model);
        }

        [HttpGet]
        public IActionResult Index()
        {
            if(orderService.TryGetModel(out var model))
                return View(model);

            return View("Empty");
        }

        [HttpPost]
        public IActionResult AddItem(int productId, int count = 1)
        {
            orderService.AppProduct(productId, count);

            return RedirectToAction("Index", "Product", new {id = productId});
        }

        [HttpPost]
        public IActionResult UpdateItem(int productId, int count)
        {
            var model = orderService.UpdateProduct(productId, count);

            return View(model);
        }

        [HttpPost]
        public IActionResult RemoveItem(int productId)
        {
            var model = orderService.RemoveProduct(productId);

            return View("Index", model);
        }

        [HttpPost]
        public IActionResult ConfirmateCellPhone(string cellPhone, int confirmationCode)
        {
            var model = orderService.ConfirmCellPhone(cellPhone, confirmationCode);

            if(model.Errors.Count > 0)
                return View("Confirmation", model);


            var deliveryMethod = deliveryServices.ToDictionary(service => service.Name,
                                                               servise => servise.Title);

            return View("DeliveryMethod", deliveryMethod);
        }

        [HttpPost]
        public IActionResult StartDelivery(string serviceName)
        {
            var deliveryMethod = deliveryServices.Single(service => service.Name == serviceName);
            var order = orderService.GetOrder();
            var form = deliveryMethod.FirstForm(order);

            var webContractorService = webContractorServices.SingleOrDefault(service => service.Name == serviceName);//??
            if(webContractorService == null)
                return View("DeliveryStep", form);

            var returnUri = GetReturnUri(nameof(NextDelivery));
            var redirectUri = webContractorService.StartSession(form.Parameters, returnUri);

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
        public IActionResult NextDelivery(string serviceName, int step, Dictionary<string, string> values)
        {
            var deliverService = deliveryServices.Single(service => service.Name == serviceName);

            var form = deliverService.NextForm(step, values);

            if(!form.IsFinal)
                return View("DeliveryStep", form);

            var delivery = deliverService.GetDelivery(form);
            orderService.SetDelivery(delivery);

            var paymantMethods = paymentServices.ToDictionary(service => service.Name,
                                                                         servise => servise.Title);

            return View("PaymentMethod", paymantMethods);
        }

        [HttpPost]
        public IActionResult StartPayment(string serviceName, int step, Dictionary<string, string> values)
        {
            var paymentService = paymentServices.Single(service => service.Name == serviceName);
            var order = orderService.GetOrder();
            var form = paymentService.FirstForm(order);
            if (!form.IsFinal)
                return View("PaymentStep", form);

            var payment = paymentService.GetPayment(form);
            var model = orderService.SetPayment(payment);

            return View("Finish", model);
        }

        [HttpPost]
        public IActionResult NextPayment(string serviceName, int step, Dictionary<string, string> values)
        {
            var paymentService = paymentServices.Single(service => service.Name == serviceName);

            var form = paymentService.NextForm(step, values);
            if (!form.IsFinal)
                return View("PaymentStep", form);

            var payment = paymentService.GetPayment(form);
            var model = orderService.SetPayment(payment);

            return View("Finish", model);
        }
    }
}
