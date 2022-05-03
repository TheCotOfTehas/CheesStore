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
        private readonly List<OrderItem> items;
        public IReadOnlyCollection<OrderItem> Items 
        {
            get { return items; }
        }

        public string CellPhone { get; set; }

        public OrderDelivery Delivery { get; set; }

        public OrderPayment Payment { get; set; }
        public decimal TotatalCount => items.Sum(item => item.Count); 

        public decimal TotalPrice => items
            .Sum(items => items.Price * items.Count) 
            + (Delivery?.Amount ?? 0m);

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

        public void RemoveItem(int productId)
        {
            int index = items.FindIndex(items => items.ProductId == productId);

            if (index == -1)
                ThrowProductException("Order does not conteins item", productId);

            items.RemoveAt(index);
        }
        
        public OrderItem GetItem(int bookId)
        {
            int index = items.FindIndex(item => item.ProductId == bookId);

            if (index == -1) ThrowProductException("Book not found.", bookId);

            return items[index];
        }

        public void AddOrUpdateItem(Product product, int count)
        {
            if (product == null)
                throw new ArgumentNullException(nameof(product), "don't be null");

            var index = items.FindIndex(item => item.ProductId == product.Id);

            if (index == -1)
                items.Add(new OrderItem(product.Id, count, product.Price));
            else
                items[index].Count += count;
        }

        private static void ThrowProductException(string messege, int produktId)
        {
            var exception = new InvalidOperationException(messege);

            exception.Data["produktId"] = produktId;

            throw exception;
        }
    }
}
