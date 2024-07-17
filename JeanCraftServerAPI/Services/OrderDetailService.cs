using AutoMapper;
using JeanCraftLibrary.Entity;
using JeanCraftLibrary;
using JeanCraftServerAPI.Services.Interface;
using System.Linq.Expressions;
using JeanCraftLibrary.Model;
using JeanCraftLibrary.Model.Request;

namespace JeanCraftServerAPI.Services
{
    public class OrderDetailService : IOrderDetailService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public OrderDetailService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task Add(ListOrderDetailUpdateRequestModel orderDetailFormModel)
        {
            try
            {
                foreach (var x in orderDetailFormModel.OrderDetails) {
                    var orderDetailEntity = _mapper.Map<OrderDetail>(x);
                    var repos = _unitOfWork.OrderDetailRepository;
                    await repos.AddAsync(orderDetailEntity);
                   
                }
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
                var repos = _unitOfWork.OrderDetailRepository;
                var orderDetailEntity = await repos.GetAsync(a => a.OrderId == orderId);
                if (orderDetailEntity == null)
                    throw new KeyNotFoundException();

                await repos.DeleteAsync(orderDetailEntity);
                await _unitOfWork.CommitAsync();
            }
            catch (Exception e)
            {
                await _unitOfWork.RollbackAsync();
                throw;
            }
        }

        public async Task<IList<OrderDetailFormModel>> GetAll()
        {
            var orderDetails = await _unitOfWork.OrderDetailRepository.GetAllAsync();
            return _mapper.Map<IList<OrderDetailFormModel>>(orderDetails);
        }

        public OrderDetailFormModel GetDetailOne(Guid id,int currentPage, int pageSize)
        {
            Expression<Func<OrderDetail, bool>> filter = orderDetail => orderDetail.OrderId == id;
            Func<IQueryable<OrderDetail>, IOrderedQueryable<OrderDetail>> orderBy = null;
            var orderDetailEntity = _unitOfWork.OrderDetailRepository.GetDetail(filter, orderBy, "Product", currentPage, pageSize).FirstOrDefault();
            return _mapper.Map<OrderDetailFormModel>(orderDetailEntity);
        }

        public async Task<OrderDetailFormModel> GetOne(Guid orderId)
        {
            var orderDetailEntity = await _unitOfWork.OrderDetailRepository.FindAsync(orderId);
            return _mapper.Map<OrderDetailFormModel>(orderDetailEntity);
        }
        public IList<OrderDetailFormModel> GetAllPaging(int currentPage, int pageSize)
        {

            Func<IQueryable<OrderDetail>, IOrderedQueryable<OrderDetail>> orderBy = null;
            var orderEntity = _unitOfWork.OrderDetailRepository.GetDetail(null, orderBy, "Product", currentPage, pageSize);
            return _mapper.Map<IList<OrderDetailFormModel>>(orderEntity);
        }

        public async Task Update(OrderDetailUpdateRequestModel orderDetailFormModel)
        {
            try
            {
                var repos = _unitOfWork.OrderDetailRepository;
                var existingOrderDetail = await repos.FindAsync(orderDetailFormModel.OrderId);
                if (existingOrderDetail == null)
                    throw new KeyNotFoundException();
                existingOrderDetail.OrderId = orderDetailFormModel.OrderId;
                existingOrderDetail.ProductId = orderDetailFormModel.ProductId;
                existingOrderDetail.PriceUnit = orderDetailFormModel.PriceUnit;
                existingOrderDetail.Quantity = orderDetailFormModel.Quantity;
                _mapper.Map(orderDetailFormModel, existingOrderDetail);

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
    }
}
