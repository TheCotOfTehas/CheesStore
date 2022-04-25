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
        private decimal count;
        public decimal Count 
        {
            get { return count; }
            set 
            {
                ThrowIfValidateCount(value);
                count = value;
            }
        }
        public decimal Price { get; }


        public OrderItem(int productId, decimal count, decimal price)
        {
            ThrowIfValidateCount(count);

            ProductId = productId;
            Count = count;
            Price = price;
        }

        private static void ThrowIfValidateCount(decimal count)
        {
            if (count <= 0)
                throw new ArgumentOutOfRangeException(nameof(count), "Count do not be less than zero");
        }
    }
}
