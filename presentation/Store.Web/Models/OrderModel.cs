namespace Store.Web.Models
{
    public class OrderModel
    {
        public int Id { get; set; }
        public OrderItemModel[] Items { get; set; } = new OrderItemModel[0];
        public decimal ToralCount { get; set; }

        public decimal TotalPrice { get; set; }
    }
}
