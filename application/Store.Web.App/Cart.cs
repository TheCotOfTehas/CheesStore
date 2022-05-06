namespace Store.Web.App
{
    public class Cart
    {
        public int OrderId { get; }
        public decimal TotalCount { get; }
        public decimal TotalPrice { get; }

        public Cart(int orderId, decimal totalCount, decimal totalPrice)
        {
            OrderId = orderId;
            TotalCount = totalCount;
            TotalPrice = totalPrice;
        }
    }
}
