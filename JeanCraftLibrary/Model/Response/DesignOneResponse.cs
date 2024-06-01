using JeanCraftLibrary.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JeanCraftLibrary.Model.Response
{
    public class DesignOneResponse
    {
        public Guid DesignOneId { get; set; }
        public Guid? Fit { get; set; }
        public Guid? Length { get; set; }
        public Guid? Cuffs { get; set; }
        public Guid? Fly { get; set; }
        public Guid? FrontPocket { get; set; }
        public Guid? BackPocket { get; set; }
        public Component? FitNavigation { get; set; }
        public Component? LengthNavigation { get; set; }
        public Component? CuffsNavigation { get; set; }
        public Component? FlyNavigation { get; set; }
        public Component? FrontPocketNavigation { get; set; }
        public Component? BackPocketNavigation { get; set; }
    }
}
