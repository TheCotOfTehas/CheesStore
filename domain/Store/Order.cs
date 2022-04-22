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
        private List<OrderItem> items;
        public IReadOnlyCollection<OrderItem> Items 
        {
            get { return items; }
        }

        public decimal TotatalCount
        {
            get { return items.Sum(item => item.Count); } 
        }

        public decimal TotalPrice
        {
            get { return items.Sum(items => items.Price * items.Count);}
        }
        public Order(int id, IEnumerable<OrderItem> items)
        {
            if (items == null)
                throw new ArgumentNullException(nameof(items), "don't be null");

            Id = id;

            this.items = new List<OrderItem>(items);
        }

        public void AddItem(Product product, int count)
        {
            if (product == null)
                throw new ArgumentNullException(nameof(product), "don't be null");

            var element = items.SingleOrDefault(item => item.ProductId == product.Id);

            if (element == null)
                items.Add(new OrderItem(product.Id, count, product.Price));
            else
            {
                items.Remove(element);
                items.Add(new OrderItem(product.Id, element.Count + count, product.Price));
            }
        }
    }
}
