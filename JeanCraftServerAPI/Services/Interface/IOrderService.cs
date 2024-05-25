using JeanCraftLibrary.Entity;

namespace JeanCraftServerAPI.Services.Interface
{
   
    public interface IOrderService
    {
        Task<IList<Order>> GetAll();
        Task<Order> GetOne(Guid OrderId);

        Task Update(Order Order);
        Task Add(Order Order);
        Task Delete(Guid Order);

        Order GetDetailOne(Guid id);
    }
}
