using JeanCraftLibrary.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JeanCraftLibrary.Model.Request
{
    public class CreateProductByBookingRequest
    {
        public string? Image { get; set; }

        public Guid? DesignOneId { get; set; }

        public Guid? DesignTwoId { get; set; }

        public double? Price { get; set; }

        public double? Size { get; set; }

        public virtual DesignThree? DesignThree { get; set; }
    }
}
