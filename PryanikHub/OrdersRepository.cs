using Microsoft.EntityFrameworkCore;
using PryanikHub.Entities;

namespace PryanikHub
{
    public interface IOrdersRepository
    {
        public IEnumerable<Order> OrdersFullInclude { get; }

        Task AddRangeAsync(List<Order> createdOrders);

        public IEnumerable<Order> GetInterval(int start, int count);
        Task RemoveByIdAsync(int uid);
        Task RemoveByIdsAsync(int[] uids);
        Task<bool> SaveChangesAsync();
    }

    public class OrdersRepository : IOrdersRepository
    {
        private readonly CoreDbContext context;

        // All includes
        public IEnumerable<Order> OrdersFullInclude
        {
            get
            {
                return context.Orders.Include(x => x.OrderLines).ThenInclude(x => x.Product);
            }
        }

        public OrdersRepository(CoreDbContext context)
        {
            this.context = context;
        }

        public async Task AddRangeAsync(List<Order> createdOrders)
        {
            await context.AddRangeAsync(createdOrders);

            await context.SaveChangesAsync();
        }

        public IEnumerable<Order> GetInterval(int start, int count)
        {
            return context.Orders.Include(x => x.OrderLines).ThenInclude(x => x.Product).Skip(start).Take(count);
        }

        public async Task RemoveByIdAsync(int id)
        {
            Order order = await context.Orders.SingleOrDefaultAsync(x => x.OrderId == id);

            if (order != null)
            {
                this.context.Orders.Remove(order);
            }
        }

        public async Task RemoveByIdsAsync(int[] ids)
        {
            await context.Orders
                .Where(x => ids.Contains(x.OrderId))
                .ExecuteDeleteAsync();
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await this.context.SaveChangesAsync() > 0;
        }
    }
}
