using JeanCraftLibrary.Entity;
using JeanCraftLibrary.Repositories.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JeanCraftLibrary.Repositories
{
    public class OrderRepository : GenericRepository<Order>, IOrderRepository
    {

        private readonly JeanCraftContext _context;
        public OrderRepository(JeanCraftContext dbContext) : base(dbContext)
        {
            _context = dbContext;
        }

        public async Task<List<OrderDetail>> GetOrderDetailByOrderID(Guid orderID)
        {
            return await _context.OrderDetails.Where(x => x.OrderId == orderID).Include(x => x.Product).ToListAsync();
        }

        public async Task<int> GetOrderCountByDateAsync(DateTime date)
        {
            var orderCount = await _context.Set<Order>()
                    .Where(o => o.CreateDate.Date == date.Date)
                    .CountAsync();
            return orderCount;
        }
    }
}
