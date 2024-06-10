using JeanCraftLibrary.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JeanCraftLibrary.Model.Response
{
    public class ShoppingCartItemResponse
    {
        public Guid CartId { get; set; }

        public Guid UserId { get; set; }

        public virtual CartItemResponse Cart { get; set; } = null!;

        public virtual Account User { get; set; } = null!;
    }
}
