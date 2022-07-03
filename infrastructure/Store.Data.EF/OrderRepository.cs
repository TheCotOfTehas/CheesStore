using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Data.EF
{
    public class OrderRepository : IOrderRepository
    {
        private readonly DbContextFactory dbContextFactory;

        public OrderRepository(DbContextFactory dbContextFactory)
        {
            this.dbContextFactory = dbContextFactory;
        }
        public async Task<Order> CreateAsync()
        {
            var context = dbContextFactory.Create(typeof(OrderRepository));

            var dto = Order.DtoFactory.Create();
            context.Orders.Add(dto);
            await context.SaveChangesAsync();

            return Order.Mapper.Map(dto);
        }

        public async Task<Order> GetByIdAsync(int id)
        {
            var context = dbContextFactory.Create(typeof(OrderRepository));

            var dto = await context.Orders
                               .Include(order => order.Items)
                               .SingleAsync(order => order.Id == id);

            return Order.Mapper.Map(dto);
        }

        public async Task UpdateAsync(Order order)
        {
            var dbContext = dbContextFactory.Create(typeof(OrderRepository));

            await dbContext.SaveChangesAsync();
        }
    }
}
