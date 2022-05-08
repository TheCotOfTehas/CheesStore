using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Store.Tests
{
    public class OrderTest
    {
        [Fact]
        public void Order_WithNullItems()
        {
            Assert.Throws<ArgumentNullException>(() => new Order(1, null));
        }

        [Fact]
        public void TotalCount_WithEmpatyItems()
        {
            var order = new Order(1, Array.Empty<OrderItem>());

            Assert.Equal(0m, order.TotatalCount);
        }
        
        [Fact]
        public void TotalSumPriseWithNonEmpatyItems()
        {
            var listItem = new List<OrderItem>()
            {
                new OrderItem(1, 1, 50),
                new OrderItem(1, 1, 100),
                new OrderItem(1, 2, 150),
            };
            var items = new Order(1, listItem);

            Assert.Equal(450m, items.TotalPrice);
        }

        [Fact]
        public void TotalCount_WithNonEmpatyItems()
        {
            var listItem = new List<OrderItem>()
            {
                new OrderItem(1, 1, 50),
                new OrderItem(1, 5, 100),
                new OrderItem(1, 2, 150),
            };
            var items = new Order(1, listItem);

            Assert.Equal(1 + 5 + 2, items.TotatalCount);
        }

        [Fact]
        public void TotalPrice_WithEmptyItems_ReturnsZero()
        {
            var order = new Order(1, new List<OrderItem>()
            {
                new OrderItem(1,6m, 100m),
                new OrderItem(2,1m, 100m),
                new OrderItem(3,1m, 100m),
            });

            Assert.Equal(100m * 6 + 1*100 +1*100, order.TotalPrice);
        }
    }
}
