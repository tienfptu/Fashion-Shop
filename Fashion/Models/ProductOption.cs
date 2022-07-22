using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Fashion.Models
{
    [Table("ProductOptions")]
    public class ProductOption
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public int ProductId { set; get; }
        [Required]
        public int ColorId { set; get; }
        [Required]
        public int SizeId { set; get; }
        [ForeignKey("ProductId")]
        public virtual Product Product { set; get; }
        [ForeignKey("ColorId")]
        public virtual Color Color { set; get; }
        [ForeignKey("SizeId")]
        public virtual Size Size { set; get; }
        public int Count { get; set; }
        public DateTime CreatedDate { get; set; }

    }
}