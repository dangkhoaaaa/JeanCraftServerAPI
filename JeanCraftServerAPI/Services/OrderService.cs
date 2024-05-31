using AutoMapper;
using JeanCraftLibrary;
using JeanCraftLibrary.Entity;
using JeanCraftLibrary.Model;
using JeanCraftLibrary.Model.Request;
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

        public async Task Add(OrderCreateRequestModel orderFormModel)
        {
            try
            {
                var orderEntity = _mapper.Map<Order>(orderFormModel);

                var repos = _unitOfWork.OrderRepository;

                // Ensure the generated ID is unique
                Guid newGuid;
                do
                {
                    newGuid = Guid.NewGuid();
                   var exist= await repos.FindAsync(newGuid);
                    if (exist == null)
                    {
                        break;
                    }
                } while (true);

                orderEntity.Id = newGuid;

                await repos.AddAsync(orderEntity);
                await _unitOfWork.CommitAsync();
            }
            catch (Exception)
            {
                await _unitOfWork.RollbackAsync();
                throw;
            }
        }

        public async Task Delete(Guid orderId)
        {
            try
            {
                var orderRepository = _unitOfWork.OrderRepository;
                var orderDetailRepository = _unitOfWork.OrderDetailRepository;

                var orderEntity = await orderRepository.GetAsync(a => a.Id == orderId);
                if (orderEntity == null)
                {
                    throw new KeyNotFoundException("Order not found");
                }

                // Fetch and delete related OrderDetail entities
                var orderDetails = await orderDetailRepository.GetAllAsync(od => od.OrderId == orderId);
                if (orderDetails != null)
                {
                    foreach (var orderDetail in orderDetails)
                    {
                        await orderDetailRepository.DeleteAsync(orderDetail);
                    }
                }

                // Delete the order itself
                await orderRepository.DeleteAsync(orderEntity);

                // Commit the transaction
                await _unitOfWork.CommitAsync();
            }
            catch (Exception)
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

        public async Task Update(OrderUpdateRequestModel orderFormModel)
        {
            try
            {
                var repos = _unitOfWork.OrderRepository;
                var existingOrder = await repos.FindAsync(orderFormModel.Id);
                if (existingOrder == null)
                    throw new KeyNotFoundException();
                existingOrder.Id = orderFormModel.Id;
                existingOrder.Status = orderFormModel.Status;
                existingOrder.AddressId = orderFormModel.AddressId;
                existingOrder.CartCost = orderFormModel.CartCost;
                existingOrder.ShippingCost = orderFormModel.ShippingCost;
                existingOrder.Note = orderFormModel.Note;
                existingOrder.UserId = orderFormModel.UserId;
                existingOrder.TotalCost = orderFormModel.TotalCost;
               
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
