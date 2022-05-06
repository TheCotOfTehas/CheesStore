﻿namespace Store.Web.App
{
    public class OrderModel
    {
        public int Id { get; set; }
        public OrderItemModel[] Items { get; set; } = Array.Empty<OrderItemModel>();
        public decimal ToralCount { get; set; }

        public decimal TotalPrice { get; set; }

        public string CellPhone { get; set; }
        public string DeliveryDescription { get; set; }
        public string PaymentDescription { get; set; }
        public Dictionary<string, string> Errors { get; set; } = new Dictionary<string, string>();
    }
}
