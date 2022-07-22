using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Fashion.Models
{
    [Table("Products")]

    public class Product
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { set; get; }
        [Required]
        [MaxLength(256)]
        public string Name { set; get; }
        [Required]
        [MaxLength(256)]
        public string Alias { set; get; }
        [Required]
        public int CategoryId { set; get; }
        [MaxLength(256)]
        public string Image { set; get; }
        public float OriginalPrice { get; set; }
        public float Price { set; get; }
        public float? PromotionPrice { set; get; }
        [MaxLength(500)]
        public string Description { set; get; }
        public string Content { set; get; }
        public bool? ActivePromotion { set; get; }
        public string QrCode { get; set; }
        public bool? HotFlag { set; get; }
        public int? ViewCount { set; get; }
        public bool Status { get; set; }
        [ForeignKey("CategoryId")]
        public virtual Category Category { set; get; }
        public virtual IEnumerable<ProductTag> ProductTags { set; get; }
        public virtual IEnumerable<ProductOption> ProductOptions { set; get; }
        public DateTime CreatedDate { get; set; }

    }
}