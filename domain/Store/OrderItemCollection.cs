using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store
{
    public class OrderItemCollection : IReadOnlyCollection<OrderItem>
    {
        private readonly List<OrderItem> items;

        public OrderItemCollection(IEnumerable<OrderItem> items)
        {
            if (items == null)
                throw new ArgumentNullException(nameof(items));

            this.items = new List<OrderItem>(items);
        }
        public int Count => items.Count;

        public OrderItem Get(int productId)
        {
            if (TryGet(productId, out OrderItem orderItem))
                return orderItem;

            throw new InvalidOperationException("Product not found.");
        }

        public bool TryGet(int productId, out OrderItem? orderItem)
        {
            var index = items.FindIndex(x => x.ProductId == productId);

            if (index == -1)
            {
                orderItem = null;
                return false;
            }

            orderItem = items[index];
            return true;
        }

        public OrderItem Add(int productId, decimal price, int count)
        {
            if (TryGet(productId, out OrderItem orderItem))
                return orderItem;

            orderItem = new OrderItem(productId, count, price);
            items.Add(orderItem);

            return orderItem; 
        }

        public void Remove( int productId)
        {
            items.Remove(Get(productId));
        }

        public IEnumerator<OrderItem> GetEnumerator()
        {
            return items.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return (items as IEnumerable).GetEnumerator();
        }
    }
}
