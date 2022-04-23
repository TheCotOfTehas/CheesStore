namespace Store.Web.Models
{
    public class OrderItemModel
    {
        public int ProductId { get; set; }
        public string Titel { get; set; }
        public string Categore { get; set; }
        public decimal Count { get; set; }
        public decimal Price { get; set; }
        public string Manufacturer { get; set; }
    }
}
