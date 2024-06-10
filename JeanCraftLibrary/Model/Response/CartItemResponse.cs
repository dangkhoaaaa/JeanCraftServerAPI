using JeanCraftLibrary.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JeanCraftLibrary.Model.Response
{
    public class CartItemResponse
    {
        public Guid ProductId { get; set; }

        public Guid Id { get; set; }

        public int? Quantity { get; set; }

        public virtual Product Product { get; set; } = null!;

        public double? Size { get; set; }
    }
}
