using JeanCraftLibrary.Entity;
using JeanCraftLibrary.Model;
using JeanCraftLibrary.Model.Request;

namespace JeanCraftServerAPI.Services.Interface
{
  
    public interface IOrderDetailService
    {
        Task<IList<OrderDetailFormModel>> GetAll();
        Task<OrderDetailFormModel> GetOne(Guid OrderDetailId);

        Task Update(OrderDetailUpdateRequestModel OrderDetail);
        Task Add(OrderDetailCreateRequestModel OrderDetail);
        Task Delete(Guid OrderDetail);

        OrderDetailFormModel GetDetailOne(Guid id, int currentPage, int pageSize);

        IList<OrderDetailFormModel> GetAllPaging(int currentPage, int pageSize);
    }
}
