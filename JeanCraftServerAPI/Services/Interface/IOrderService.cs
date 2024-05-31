using JeanCraftLibrary.Entity;
using JeanCraftLibrary.Model;
using JeanCraftLibrary.Model.Request;

namespace JeanCraftServerAPI.Services.Interface
{
   
    public interface IOrderService
    {
        Task<IList<OrderFormModel>> GetAll();
        Task<OrderFormModel> GetOne(Guid OrderId);

        Task Update(OrderUpdateRequestModel Order);
        Task Add(OrderCreateRequestModel Order);
        Task Delete(Guid Order);

        OrderFormModel GetDetailOne(Guid id, int currentPage, int pageSize);
        IList<OrderFormModel> GetAllPaging(int currentPage, int pageSize);
    }
}
