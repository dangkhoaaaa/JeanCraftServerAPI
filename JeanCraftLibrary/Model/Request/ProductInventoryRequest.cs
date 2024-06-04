using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JeanCraftLibrary.Model.Request
{
    public class ProductInventoryRequest
    {
        public Guid ProductInventoryId { get; set; }

        public int? Quantity { get; set; }
    }
}
