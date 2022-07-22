using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Fashion.Models
{ 
    [Table("OrderDetails")]
    public class OrderDetail
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int OrderId { set; get; }
        public int ProductId { set; get; }
        public int Quantity { set; get; }
        public int ColorId { get; set; }
        public int SizeId { get; set; }
        public float Price { get; set; }
        [ForeignKey("OrderId")]
        public virtual Order Order { set; get; }
        [ForeignKey("ProductId")]
        public virtual Product Product { set; get; }
    }
}