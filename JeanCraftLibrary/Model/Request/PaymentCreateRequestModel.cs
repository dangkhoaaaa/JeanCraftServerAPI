using JeanCraftLibrary.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JeanCraftLibrary.Model.Request
{
    public class PaymentCreateRequestModel
    {
        public Guid Id { get; set; }

        public Guid? OrderId { get; set; }

        public double? Amount { get; set; }

        public string? Method { get; set; }

        public string? Status { get; set; }

      //  public virtual Order? Order { get; set; }
    }
}
