using JeanCraftLibrary.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JeanCraftLibrary.Model.Request
{
    public class OrderDetailCreateRequestModel
    {
        public Guid OrderId { get; set; }

        public Guid ProductId { get; set; }

        public double? PriceUnit { get; set; }

        public int? Quantity { get; set; }


    }
}
