using JeanCraftLibrary.Entity;
using JeanCraftLibrary;
using JeanCraftServerAPI.Services.Interface;
using System.Linq.Expressions;

namespace JeanCraftServerAPI.Services
{
  
    public class OrderDetailService : IOrderDetailService
    {
        private readonly IUnitOfWork _unitOfWork;

        public OrderDetailService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task Add(OrderDetail Order)
        {
            try
            {

                var repos = _unitOfWork.OrderDetailRepository;
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

                var repos = _unitOfWork.OrderDetailRepository;
                var Order = await repos.GetAsync(a => a.OrderId == OrderId);
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

        public async Task<IList<OrderDetail>> GetAll()
        {
            return await _unitOfWork.OrderDetailRepository.GetAllAsync();
        }


        public OrderDetail GetDetailOne(Guid id)
        {

            Expression<Func<OrderDetail, bool>> filter = user => user.OrderId == id;
            Func<IQueryable<OrderDetail>, IOrderedQueryable<OrderDetail>> orderBy = null;
            var x = _unitOfWork.OrderDetailRepository.GetDetail(filter, orderBy, "Product", null, null).FirstOrDefault();
            return x;
        }

        public async Task<OrderDetail> GetOne(Guid OrderId)
        {
            return await _unitOfWork.OrderDetailRepository.FindAsync(OrderId);
        }


        public async Task Update(OrderDetail OrderDetail)
        {
            try
            {

                var repos = _unitOfWork.OrderDetailRepository;
                var a = await repos.FindAsync(OrderDetail.OrderId);
                if (a == null)
                    throw new KeyNotFoundException();
                a.ProductId = OrderDetail.ProductId;
            

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
