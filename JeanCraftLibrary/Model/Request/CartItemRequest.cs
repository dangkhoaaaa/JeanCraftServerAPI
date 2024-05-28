using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JeanCraftLibrary.Model.Request
{
    public class CartItemRequest
    {
        public Guid? ProductId { get; set; }


        public int? Quantity { get; set; }
    }
}
