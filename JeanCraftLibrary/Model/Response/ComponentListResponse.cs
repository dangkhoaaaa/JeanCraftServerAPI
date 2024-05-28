using JeanCraftLibrary.Entity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JeanCraftLibrary.Model.Response
{
    public class ComponentListResponse
    {
        public List<ComponentForType> ListComponentForType { get; set; }
    }
    public class ComponentForType
    {
        public Guid Type { get; set; }
        public IEnumerable<Component> Components { get; set; }
    }
}
