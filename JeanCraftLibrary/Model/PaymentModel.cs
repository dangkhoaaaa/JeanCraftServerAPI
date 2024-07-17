using JeanCraftLibrary.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JeanCraftLibrary.Model
{
    public class PaymentModel
    {
        public Guid? Id { get; set; }

        public Guid? OrderId { get; set; }

        public double Amount { get; set; }
        public String? RedirectUrl { get; set; }
    }

}
