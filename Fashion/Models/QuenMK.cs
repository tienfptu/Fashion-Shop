using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Fashion.Models
{
    public class QuenMK
    {
        [Required(ErrorMessage = "Vui lòng nhập tên tài khoản")]
        public string username { set; get; }
    }
}