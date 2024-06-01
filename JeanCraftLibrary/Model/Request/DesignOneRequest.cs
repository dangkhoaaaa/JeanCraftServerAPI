using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JeanCraftLibrary.Model.Request
{
    public class DesignOneRequest
    {
        public Guid? Fit { get; set; }
        public Guid? Length { get; set; }
        public Guid? Cuffs { get; set; }
        public Guid? Fly { get; set; }
        public Guid? FrontPocket { get; set; }
        public Guid? BackPocket { get; set; }
    }
}
