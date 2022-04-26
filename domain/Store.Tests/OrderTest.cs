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
        public void AddItem_WithNullProduct()
        {
            var order = new Order(1, new List<OrderItem>());
            Assert.Throws<ArgumentNullException>(() => order.AddItem(null, 2));
        }

        [Fact]
        public void AddItem_WithConteinsProduct()
        {
            int idAddProd1 = 3;
            var addItem = new Product(idAddProd1, "Качокавалла", "Chees", "Podoksha", "Тут описание", 190m);
            var order = new Order(1, new List<OrderItem>()
            {
                new OrderItem(1,1m, 100m),
                new OrderItem(2,1m, 100m),
                new OrderItem(3,1m, 100m),
            });
            order.AddItem(addItem, 5);
            
            Assert.Equal(8, order.TotatalCount);
            Assert.Equal(6, order.Items.Single(x => x.ProductId == idAddProd1).Count);
        }


        [Fact]
        public void AddItem_NewProduct()
        {
            int idAddProd1 = 4;
            var addItem = new Product(idAddProd1, "Качокавалла", "Chees", "Podoksha", "Тут описание", 190m);
            var order = new Order(1, new List<OrderItem>()
            {
                new OrderItem(1,1m, 100m),
                new OrderItem(2,1m, 100m),
                new OrderItem(3,1m, 100m),
            });
            order.AddItem(addItem, 5);

            Assert.Equal(8, order.TotatalCount);
            Assert.Equal(4, order.Items.Count);
            Assert.Equal(5, order.Items.Single(x => x.ProductId == idAddProd1).Count);
        }

        [Fact]
        public void GetItem_WithExistingItem_ReturnsItem()
        {
            var order = new Order(1, new List<OrderItem>()
            {
                new OrderItem(1,6m, 100m),
                new OrderItem(2,1m, 100m),
                new OrderItem(3,1m, 100m),
            });

            var item = order.GetItem(1);

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
                order.GetItem(4);
            });
        }
        [Fact]
        public void AddOrUpdateItem_withExistingItem_UpdatesCount()
        {
            var order = new Order(1, new List<OrderItem>()
            {
                new OrderItem(1,6m, 100m),
                new OrderItem(2,1m, 100m),
                new OrderItem(3,1m, 100m),
            });

            var product = new Product(1, null, null, null, null, 0m);

            order.AddOrUpdateItem(product, 10);
            Assert.Equal(16, order.GetItem(1).Count);
        }

        [Fact]
        public void AddOrUpdateItem_witNonExistingItem_UpdatesCount()
        {
            var order = new Order(1, new List<OrderItem>()
            {
                new OrderItem(1,6m, 100m),
                new OrderItem(2,3m, 100m),
                new OrderItem(3,1m, 100m),
            });

            var product = new Product(4, null, null, null, null, 0m);

            order.AddOrUpdateItem(product, 10);
            Assert.Equal(10, order.GetItem(4).Count);

        }

        [Fact]
        public void RemoveItem_WithExistingItem_RemoveItem()
        {
            var order = new Order(1, new List<OrderItem>()
            {
                new OrderItem(1,6m, 100m),
                new OrderItem(2,1m, 100m),
                new OrderItem(3,1m, 100m),
            });

            order.RemoveItem(1);
            Assert.Equal(2, order.Items.Count);
        }

        [Fact]
        public void RemoveItem_WithNonExistingItem_RemoveItem()
        {
            var order = new Order(1, new List<OrderItem>()
            {
                new OrderItem(1,6m, 100m),
                new OrderItem(2,1m, 100m),
                new OrderItem(3,1m, 100m),
            });

            Assert.Throws<InvalidOperationException>(() => order.RemoveItem(4));
        }
    }
}
