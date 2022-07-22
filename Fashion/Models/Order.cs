using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Fashion.Models
{
    [Table("Orders")]
    public class Order
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { set; get; }
        [Required]
        [MaxLength(256)]
        public string CustomerName { set; get; }
        [Required]
        [MaxLength(256)]
        public string CustomerAddress { set; get; }
        [Required]
        [MaxLength(256)]
        public string CustomerEmail { set; get; }
        [Required]
        [MaxLength(50)]
        public string CustomerMobile { set; get; }
        [Required]
        [MaxLength(256)]
        public string CustomerMessage { set; get; }
        public DateTime CreatedDate { set; get; }
        public int CreatedBy { set; get; }
        public int Status { set; get; }
        public float Total { get; set; }
        public string Code { get; set; }
        public int CustomerId { set; get; }

        [ForeignKey("CustomerId")]
        public virtual Customer Customer { set; get; }

        public virtual IEnumerable<OrderDetail> OrderDetails { set; get; }
    }
}