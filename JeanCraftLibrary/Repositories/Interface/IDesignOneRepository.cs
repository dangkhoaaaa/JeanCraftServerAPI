using JeanCraftLibrary.Entity;
using JeanCraftLibrary.Model.Request;
using JeanCraftLibrary.Model.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JeanCraftLibrary.Repositories.Interface
{
    public interface IDesignOneRepository : IGenericRepository<DesignOne>
    {
        Task<List<DesignOne>> GetAllDesignOnes();
        Task<DesignOne> GetDesignOneById(Guid designOneId);
        Task<DesignOneResponse> CreateAsync(DesignOneRequest designOneRequest);
    }
}
