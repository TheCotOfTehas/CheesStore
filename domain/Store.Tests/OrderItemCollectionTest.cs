using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Store.Tests
{
    public class OrderItemCollectionTest
    {

        [Fact]
        public void GetItem_WithExistingItem_ReturnsItem()
        {
            var order = new Order(1, new List<OrderItem>()
            {
                new OrderItem(1,6m, 100m),
                new OrderItem(2,1m, 100m),
                new OrderItem(3,1m, 100m),
            });

            var item = order.Items.Get(1);

            Assert.Equal(6, item.Count);
        }

        [Fact]
        public void GetItem_WithNonExistingItem_ThrowsInvalidOperation()
        {
            var order = new Order(1, new List<OrderItem>()
            {
                new OrderItem(1,6m, 100m),
                new OrderItem(2,1m, 100m),
                new OrderItem(3,1m, 100m),
            });

            Assert.Throws<InvalidOperationException>(() =>
            {
                order.Items.Get(4);
            });
        }

        [Fact]
        public void Add_WithNewItem_SetsCount()
        {
            var order = new Order(1, new List<OrderItem>()
            {
                new OrderItem(1,6m, 100m),
                new OrderItem(2,1m, 100m),
                new OrderItem(3,1m, 100m),
            });

            order.Items.Add(4, 30m, 10);

            Assert.Equal(10, order.Items.Get(4).Count);
        }

        [Fact]
        public void Add_OrUpdateItem_witNonExistingItem_SetsCount()
        {
            var order = new Order(1, new List<OrderItem>()
            {
                new OrderItem(1,6m, 100m),
                new OrderItem(2,3m, 100m),
                new OrderItem(3,1m, 100m),
            });

            order.Items.Add(4, 30m, 10);

            Assert.Equal(10, order.Items.Get(4).Count);
        }

        [Fact]
        public void Remove_WithExistingItem_RemoveItem()
        {
            var order = new Order(1, new List<OrderItem>()
            {
                new OrderItem(1,6m, 100m),
                new OrderItem(2,1m, 100m),
                new OrderItem(3,1m, 100m),
            });

            order.Items.Remove(1);
            Assert.Equal(2, order.Items.Count);
        }

        [Fact]
        public void Remove_WithNonExistingItem_RemoveItem()
        {
            var order = new Order(1, new List<OrderItem>()
            {
                new OrderItem(1,6m, 100m),
                new OrderItem(2,1m, 100m),
                new OrderItem(3,1m, 100m),
            });

            Assert.Throws<InvalidOperationException>(() => order.Items.Remove(4));
        }
    }
}
