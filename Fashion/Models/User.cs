using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Fashion.Models
{
    [Table("Users")]
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; }
        [StringLength(256)]
        [Column(TypeName = "VARCHAR")]
        [Index(IsUnique = true)]
        public string Email { get; set; }
        [StringLength(20)]
        [Column(TypeName = "VARCHAR")]
        [Index(IsUnique = true)]
        public string Phone { get; set; }
        [StringLength(250)]
        [Column(TypeName = "VARCHAR")]
        [Index(IsUnique = true)]
        public string Username { get; set; }
        public string Password { get; set; }
        public bool Status { get; set; }
        public string Image { get; set; }
        public DateTime CreatedDate { get; set; }

    }
}