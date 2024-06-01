using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JeanCraftLibrary.Model.Request
{
    public class ShoppingCartRequest
    {
        public Guid CartId { get; set; }

        public Guid UserId { get; set; }
    }
}
