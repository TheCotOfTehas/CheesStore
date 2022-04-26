namespace Store.Web.Models
{
    public class OrderModel
    {
        public int Id { get; set; }
        public OrderItemModel[] Items { get; set; } = Array.Empty<OrderItemModel>();
        public decimal ToralCount { get; set; }

        public decimal TotalPrice { get; set; }
    }
}
