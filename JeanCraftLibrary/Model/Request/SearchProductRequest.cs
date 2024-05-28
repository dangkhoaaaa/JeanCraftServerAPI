using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JeanCraftLibrary.Model.Request
{
    public class SearchProductRequest
    {
        public float? MinPrice { get; set; }
        public float? MaxPrice { get; set; }
        public float? MinSize { get; set; }
        public float? MaxSize { get; set; }
    }
}
