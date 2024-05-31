using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JeanCraftLibrary.Model.Request
{
    public class ListOrderDetailUpdateRequestModel
    {
        public virtual ICollection<OrderDetailCreateRequestModel> OrderDetails { get; set; } = new List<OrderDetailCreateRequestModel>();
    }
}
