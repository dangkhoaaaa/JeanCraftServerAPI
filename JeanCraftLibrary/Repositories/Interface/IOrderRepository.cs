using JeanCraftLibrary.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JeanCraftLibrary.Repositories.Interface
{
    public interface IOrderRepository : IGenericRepository<Order>
    {
        Task<List<OrderDetail>> GetOrderDetailByOrderID(Guid orderID);
        Task<int> GetOrderCountByDateAsync(DateTime date);
    }
}
