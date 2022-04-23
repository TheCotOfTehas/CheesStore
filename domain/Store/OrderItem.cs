using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store
{
    public class OrderItem
    {
        public int ProductId { get; }
        public decimal Count { get; }
        public decimal Price { get; }


        public OrderItem(int productId, decimal count, decimal price)
        {
            if (count <= 0)
                throw new ArgumentOutOfRangeException("Count do not be less than zero", nameof(count));

            ProductId = productId;
            Count = count;
            Price = price;
        }
    }
}
