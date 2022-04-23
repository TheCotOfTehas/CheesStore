namespace Store.Web.Models
{
    public class Cart
    {
        public int OrderId { get; }
        public decimal TotalCount { get; set; }
        public decimal TotalPrice { get; set; }

        public Cart(int orderId)
        {
            OrderId = orderId;
            TotalCount = 0m;
            TotalPrice = 0m;
        }
    }
}
