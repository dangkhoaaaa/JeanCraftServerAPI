using JeanCraftLibrary.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JeanCraftLibrary.Model
{
    public class AccountDTO
    {
        public Guid UserId { get; set; }
        public string? UserName { get; set; }
        public string? Phonenumber { get; set; }
        public string? Email { get; set; }
        public string? Image { get; set; }
        public ICollection<Address> Addresses { get; set; } = new List<Address>();
    }
}
