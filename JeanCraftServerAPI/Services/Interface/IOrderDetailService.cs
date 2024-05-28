using JeanCraftLibrary.Entity;
using JeanCraftLibrary.Model;

namespace JeanCraftServerAPI.Services.Interface
{
  
    public interface IOrderDetailService
    {
        Task<IList<OrderDetailFormModel>> GetAll();
        Task<OrderDetailFormModel> GetOne(Guid OrderDetailId);

        Task Update(OrderDetailFormModel OrderDetail);
        Task Add(OrderDetailFormModel OrderDetail);
        Task Delete(Guid OrderDetail);

        OrderDetailFormModel GetDetailOne(Guid id, int currentPage, int pageSize);

        IList<OrderDetailFormModel> GetAllPaging(int currentPage, int pageSize);
    }
}
