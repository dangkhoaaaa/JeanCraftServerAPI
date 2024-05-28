using AutoMapper;
using JeanCraftLibrary;
using JeanCraftLibrary.Entity;
using JeanCraftLibrary.Model;
using JeanCraftServerAPI.Services.Interface;
using System.Linq.Expressions;

namespace JeanCraftServerAPI.Services
{
  
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public OrderService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task Add(OrderFormModel orderFormModel)
        {
            try
            {
                var orderEntity = _mapper.Map<Order>(orderFormModel);
                var repos = _unitOfWork.OrderRepository;
                await repos.AddAsync(orderEntity);
                await _unitOfWork.CommitAsync();
            }
            catch (Exception e)
            {
                await _unitOfWork.RollbackAsync();
                throw;
            }
        }

        public async Task Delete(Guid orderId)
        {
            try
            {
                var repos = _unitOfWork.OrderRepository;
                var orderEntity = await repos.GetAsync(a => a.Id == orderId);
                if (orderEntity == null)
                    throw new KeyNotFoundException();

                await repos.DeleteAsync(orderEntity);
                await _unitOfWork.CommitAsync();
            }
            catch (Exception e)
            {
                await _unitOfWork.RollbackAsync();
                throw;
            }
        }

        public async Task<IList<OrderFormModel>> GetAll()
        {
            var orders = await _unitOfWork.OrderRepository.GetAllAsync();
            return _mapper.Map<IList<OrderFormModel>>(orders);
        }

        public OrderFormModel GetDetailOne(Guid id, int currentPage, int pageSize)
        {
            Expression<Func<Order, bool>> filter = order => order.Id == id;
            Func<IQueryable<Order>, IOrderedQueryable<Order>> orderBy = null;
            var orderEntity = _unitOfWork.OrderRepository.GetDetail(filter, orderBy, "OrderDetails", currentPage, pageSize).FirstOrDefault();
            return _mapper.Map<OrderFormModel>(orderEntity);
        }
        public IList<OrderFormModel> GetAllPaging(int currentPage, int pageSize)
        {
          
            Func<IQueryable<Order>, IOrderedQueryable<Order>> orderBy = null;
            var orderEntity = _unitOfWork.OrderRepository.GetDetail(null, orderBy, "OrderDetails", currentPage, pageSize);
            return _mapper.Map<IList<OrderFormModel>>(orderEntity);
        }
        public async Task<OrderFormModel> GetOne(Guid orderId)
        {
            var orderEntity = await _unitOfWork.OrderRepository.FindAsync(orderId);
            return _mapper.Map<OrderFormModel>(orderEntity);
        }

        public async Task Update(OrderFormModel orderFormModel)
        {
            try
            {
                var repos = _unitOfWork.OrderRepository;
                var existingOrder = await repos.FindAsync(orderFormModel.Id);
                if (existingOrder == null)
                    throw new KeyNotFoundException();

                _mapper.Map(orderFormModel, existingOrder);

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
