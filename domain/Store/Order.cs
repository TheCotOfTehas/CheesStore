using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store
{
    public class Order
    {
        public int Id { get; }

        public OrderItemCollection Items { get; }

        public string CellPhone { get; set; }

        public OrderDelivery Delivery { get; set; }

        public OrderPayment Payment { get; set; }

        public decimal TotatalCount => Items.Sum(item => item.Count); 

        public decimal TotalPrice => Items
            .Sum(items => items.Price * items.Count) 
            + (Delivery?.Amount ?? 0m);

        public Order(int id, IEnumerable<OrderItem> items)
        {
            Id = id;
            this.Items = new OrderItemCollection(items);
        }

        private static void ThrowProductException(string messege, int produktId)
        {
            var exception = new InvalidOperationException(messege);

            exception.Data["produktId"] = produktId;

            throw exception;
        }
    }
}
