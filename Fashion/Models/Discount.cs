using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Fashion.Models
{
    [Table("Discounts")]
    public class Discount
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        [StringLength(256)]
        [Column(TypeName = "VARCHAR")]
        [Index(IsUnique = true)]
        public string Code { get; set; }
        public int Value { get; set; }
        public string Detail { get; set; }
        public int Time { get; set; }
        public bool Status { get; set; }
        public DateTime CreatedDate { get; set; }

    }
}