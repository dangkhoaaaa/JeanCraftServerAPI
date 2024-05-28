using JeanCraftLibrary.Entity;
using JeanCraftLibrary.Model;

namespace JeanCraftServerAPI.Services.Interface
{
   
    public interface IOrderService
    {
        Task<IList<OrderFormModel>> GetAll();
        Task<OrderFormModel> GetOne(Guid OrderId);

        Task Update(OrderFormModel Order);
        Task Add(OrderFormModel Order);
        Task Delete(Guid Order);

        OrderFormModel GetDetailOne(Guid id, int currentPage, int pageSize);
        IList<OrderFormModel> GetAllPaging(int currentPage, int pageSize);
    }
}
