using AutoMapper;
using JeanCraftLibrary;
using JeanCraftLibrary.Entity;
using JeanCraftLibrary.Model;
using JeanCraftLibrary.Model.Request;
using JeanCraftServerAPI.Services.Interface;
using JeanCraftServerAPI.Util;
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

        public async Task<Guid> Add(OrderCreateRequestModel orderFormModel)
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
                orderEntity.CreateDate = DateTime.Now.AddHours(7);
                await repos.AddAsync(orderEntity);
                await _unitOfWork.CommitAsync();
                return orderEntity.Id;
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

        public PageResult<OrderFormModel> GetAllPaging(int currentPage, int pageSize)
        {
            Func<IQueryable<Order>, IOrderedQueryable<Order>> orderBy = orders => orders.OrderByDescending(order => order.CreateDate);
            var query = _unitOfWork.OrderRepository.GetDetail(null, orderBy, "OrderDetails");

            var totalRecords = query.Count();

            var orderEntities = query.Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();

            var totalPages = (int)Math.Ceiling((double)totalRecords / pageSize);

            var orderModels = _mapper.Map<IList<OrderFormModel>>(orderEntities);

            return new PageResult<OrderFormModel>
            {
                Items = orderModels,
                TotalRecords = totalRecords,
                TotalPages = totalPages,
                CurrentPage = currentPage,
                PageSize = pageSize
            };
        }

        public int GetTotalCount()
        {
            try
            {
                return _unitOfWork.OrderRepository.GetDetail(null, null, "").Count();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public IList<OrderFormModel> GetAllPagingSucessfully(int currentPage, int pageSize)
        {

            Func<IQueryable<Order>, IOrderedQueryable<Order>> orderBy = orders => orders.OrderByDescending(order => order.CreateDate);
            var orderEntity = _unitOfWork.OrderRepository.GetDetail(null, orderBy, "OrderDetails", currentPage, pageSize).Where(x => (x.Status == null ? false : x.Status.Equals("Đã hoàn tất")));
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

        public async Task UpdateStatus(Guid id, string status)
        {
            try
            {
                var repos = _unitOfWork.OrderRepository;
                var existingOrder = await repos.FindAsync(id);
                if (existingOrder == null)
                    throw new KeyNotFoundException();
                existingOrder.Status = status;

                await _unitOfWork.CommitAsync();
            }
            catch (Exception e)
            {
                await _unitOfWork.RollbackAsync();
                throw;
            }
        }

        public async Task<List<OrderDetail>> GetOrderDetailByOrderID(Guid orderID)
        {
            return await _unitOfWork.OrderRepository.GetOrderDetailByOrderID(orderID);
        }

        public async Task<IList<OrderFormModel>> GetOrdersByDateAsync(DateTime date)
        {
            var startDate = date.Date;
            var endDate = startDate.AddDays(1);

            var orders = await _unitOfWork.OrderRepository.GetAllAsync(o => o.CreateDate >= startDate && o.CreateDate < endDate);
            return _mapper.Map<IList<OrderFormModel>>(orders);
        }

        public async Task<int> GetOrderCountByDateAsync(DateTime date)
        {
            return await _unitOfWork.OrderRepository.GetOrderCountByDateAsync(date);
        }
    }
}
