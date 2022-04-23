using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Store.Tests
{
    public class OrderItemTest
    {
        [Fact]
        public void OrderItem_WithZeroCount()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                int count = 0;
                new OrderItem(1, count, 0m);
            });
        }

        [Fact]
        public void OrderItem_WithLessZeroCount()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                int count = -1;
                new OrderItem(1, count, 0m);
            });
        }
        [Fact]
        public void OrderItem_WithMoreZeroCount()
        {
            var orderItem = new OrderItem(1, 1, 4m);
            Assert.Equal(1, orderItem.ProductId);
            Assert.Equal(1, orderItem.Count);
            Assert.Equal(4m, orderItem.Price);
        }
    }
}
