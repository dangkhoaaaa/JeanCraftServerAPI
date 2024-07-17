using JeanCraftLibrary.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JeanCraftLibrary.Model
{
    public class AccountDTOs
    {
        public Guid UserId { get; set; }
        public string? UserName { get; set; }
        public string? Phonenumber { get; set; }
        public ICollection<AddressDTO> Addresses { get; set; } = new List<AddressDTO>();
    }
}
