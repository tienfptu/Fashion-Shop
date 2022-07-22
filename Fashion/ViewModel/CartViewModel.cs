using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Fashion.ViewModel
{
    public class CartViewModel
    {
        public int ProductID { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
        public float? Price { get; set; }
        public string Image { get; set; }
        public int SizeId { get; set; }
        public int ColorId { get; set; }
        public string SizeName { get; set; }
        public string ColorName { get; set; }
    }
}