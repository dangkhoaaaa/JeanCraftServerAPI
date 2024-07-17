using JeanCraftLibrary.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JeanCraftLibrary.Model.Request
{
    public class AccountRequest
    {
        public Guid UserId { get; set; }

        public string? UserName { get; set; }

        public string? PhoneNumber { get; set; }

        public virtual ICollection<Address> Addresses { get; set; } = new List<Address>();
    }
}
