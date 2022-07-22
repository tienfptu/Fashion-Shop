using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Fashion.ViewModel
{
    public class CommentViewModel
    {
        public int id;
        public string Name { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Content { get; set; }
        public string NameProduct { get; set; }
            
    }
}