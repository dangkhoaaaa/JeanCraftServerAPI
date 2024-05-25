using JeanCraftLibrary.Entity;
using JeanCraftLibrary.Repositories.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JeanCraftLibrary.Repositories
{
    public class OrderRepository : GenericRepository<Order>, IOrderRepository
    {


        public OrderRepository(JeanCraftContext dbContext) : base(dbContext)
        {

        }



    }
}
