using JeanCraftLibrary.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JeanCraftLibrary.Model
{
    public class OrderFormModel
    {
        public Guid Id { get; set; }

        public DateTime? CreateDate { get; set; }

        public string? Status { get; set; }

        public Guid? AddressId { get; set; }

        public double? CartCost { get; set; }

        public double? ShippingCost { get; set; }

        public string? Note { get; set; }

        public Guid? UserId { get; set; }

        public string? TotalCost { get; set; }

        public virtual Address? Address { get; set; }

        public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

        public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();

        public virtual Account? User { get; set; }
    }
}
