using Fashion.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Fashion.ViewModel
{
    public class ProductViewModel
    {
        public List<Product> ProductHots { get; set; } = new List<Product>();
        public List<Product> ProductNews { get; set; } = new List<Product>();
        public List<Product> ProductPromotions { get; set; } = new List<Product>();
        public List<Product> ProductPriceTop { get; set; } = new List<Product>();
        public List<Product> ProductPriceBot { get; set; } = new List<Product>();

    }
}