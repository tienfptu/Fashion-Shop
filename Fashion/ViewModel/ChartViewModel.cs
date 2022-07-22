using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Fashion.ViewModel
{
    public class ChartViewModel
    {
        public DateTime Date { get; set; }
        public float _Order { get; set; }
        public float Sales { get; set; }
        public int Quantity { get; set; }
        public string Time { get; set; }
    }
}