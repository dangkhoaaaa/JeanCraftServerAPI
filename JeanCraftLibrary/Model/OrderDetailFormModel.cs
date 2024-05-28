using JeanCraftLibrary.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JeanCraftLibrary.Model
{
    public class OrderDetailFormModel
    {
        public Guid OrderId { get; set; }

        public Guid ProductId { get; set; }

        public double? PriceUnit { get; set; }

        public int? Quantity { get; set; }

        public virtual Order Order { get; set; } = null!;

        public virtual Product Product { get; set; } = null!;
    }
}
