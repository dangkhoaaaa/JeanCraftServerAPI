using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JeanCraftLibrary.Model
{
    public class ResetPassWordRequest
    {
        public Guid? UserID { get; set; }
        public string Email { get; set; }
    }
}
