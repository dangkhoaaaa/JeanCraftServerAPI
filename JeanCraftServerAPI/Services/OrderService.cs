using JeanCraftLibrary;
using JeanCraftLibrary.Entity;
using JeanCraftServerAPI.Services.Interface;
using System.Linq.Expressions;

namespace JeanCraftServerAPI.Services
{
  
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;

        public OrderService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task Add(Order Order)
        {
            try
            {

                var repos = _unitOfWork.OrderRepository;
                await repos.AddAsync(Order);

                await _unitOfWork.CommitAsync();
            }
            catch (Exception e)
            {
                await _unitOfWork.RollbackAsync();
                throw;
            }
        }

        public async Task Delete(Guid OrderId)
        {
            try
            {

                var repos = _unitOfWork.OrderRepository;
                var Order = await repos.GetAsync(a => a.Id == OrderId);
                if (Order == null)
                    throw new KeyNotFoundException();

                await repos.DeleteAsync(Order);

                await _unitOfWork.CommitAsync();
            }
            catch (Exception e)
            {
                await _unitOfWork.RollbackAsync();
                throw;
            }
        }

        public async Task<IList<Order>> GetAll()
        {
            return await _unitOfWork.OrderRepository.GetAllAsync();
        }


        public Order GetDetailOne(Guid id)
        {

            Expression<Func<Order, bool>> filter = user => user.Id == id;
            Func<IQueryable<Order>, IOrderedQueryable<Order>> orderBy = null;
            var x = _unitOfWork.OrderRepository.GetDetail(filter, orderBy, "OrderDetails", null, null).FirstOrDefault();
            return x;
        }

        public async Task<Order> GetOne(Guid OrderId)
        {
            return await _unitOfWork.OrderRepository.FindAsync(OrderId);
        }


        public async Task Update(Order Order)
        {
            try
            {

                var repos = _unitOfWork.OrderRepository;
                var a = await repos.FindAsync(Order.Id);
                if (a == null)
                    throw new KeyNotFoundException();
                a.ShippingCost = Order.ShippingCost;
                a.CartCost = Order.CartCost;
                a.TotalCost = Order.TotalCost;
                a.AddressId = Order.AddressId;
                //a.Name = a.Name;

                await _unitOfWork.CommitAsync();
            }
            catch (Exception e)
            {
                await _unitOfWork.RollbackAsync();
                throw;
            }
        }

    }
}
