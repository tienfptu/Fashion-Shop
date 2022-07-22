using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Fashion.ViewModel
{
    public class OptionViewModel
    {
        public int Id { get; set; }
        public int ColorId { get; set; }
        public string ColorName { get; set; }
        public int ProductId { get; set; }
        public int SizeId { get; set; }
        public string SizeName { get; set; }
    }
}