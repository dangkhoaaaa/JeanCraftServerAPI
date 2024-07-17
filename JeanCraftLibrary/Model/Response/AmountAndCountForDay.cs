using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JeanCraftLibrary.Model.Response
{
    public class AmountAndCountForDay
    {
        public string Date {  get; set; }
        public int Count { get; set; }
        public double Amount { get; set; }
    }
}
