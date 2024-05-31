using JeanCraftLibrary.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JeanCraftLibrary.Model.Request
{
    public class OrderCreateRequestModel
    {

        public DateTime? CreateDate { get; set; }

        public string? Status { get; set; }

        public Guid? AddressId { get; set; }

        public double? CartCost { get; set; }

        public double? ShippingCost { get; set; }

        public string? Note { get; set; }

        public Guid? UserId { get; set; }

        public string? TotalCost { get; set; }

       // public virtual ICollection<OrderDetailCreateRequestModel> OrderDetails { get; set; } = new List<OrderDetailCreateRequestModel>();


    }
}
