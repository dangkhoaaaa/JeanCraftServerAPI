using JeanCraftLibrary.Entity;
using JeanCraftLibrary.Model;
using JeanCraftLibrary.Model.Request;
using JeanCraftServerAPI.Util;
using System.Collections;

namespace JeanCraftServerAPI.Services.Interface
{
   
    public interface IOrderService
    {
        Task<IList<OrderFormModel>> GetAll();
        Task<OrderFormModel> GetOne(Guid OrderId);

        Task Update(OrderUpdateRequestModel Order);
        Task UpdateStatus(Guid id, string status);
        Task<Guid> Add(OrderCreateRequestModel Order);
        Task Delete(Guid Order);
        Task<IList<OrderFormModel>> GetOrdersByDateAsync(DateTime date);
        Task<int> GetOrderCountByDateAsync(DateTime date);
        OrderFormModel GetDetailOne(Guid id, int currentPage, int pageSize);
        PageResult<OrderFormModel> GetAllPaging(int currentPage, int pageSize);
        IList<OrderFormModel> GetAllPagingSucessfully(int currentPage, int pageSize);
        Task<List<OrderDetail>> GetOrderDetailByOrderID(Guid orderID);
        int GetTotalCount();
    }
}
