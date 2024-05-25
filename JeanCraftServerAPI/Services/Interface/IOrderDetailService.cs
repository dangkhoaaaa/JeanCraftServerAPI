using JeanCraftLibrary.Entity;

namespace JeanCraftServerAPI.Services.Interface
{
  
    public interface IOrderDetailService
    {
        Task<IList<OrderDetail>> GetAll();
        Task<OrderDetail> GetOne(Guid OrderDetailId);

        Task Update(OrderDetail OrderDetail);
        Task Add(OrderDetail OrderDetail);
        Task Delete(Guid OrderDetail);

        OrderDetail GetDetailOne(Guid id);
    }
}
