using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Fashion.ViewModel
{
    public class CartTotal
    {
        public float? Total { get; set; } = 0;
        public string Code { get; set; }
        public float? Payment { get; set; } = 0;
        public string Value { get; set; }
    }
}