using Fashion.Library;
using Fashion.Models;
using Fashion.ViewModel;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Fashion.Controllers
{
    public class ProductController : Controller
    {
        private FSDbContext db = new FSDbContext();
        public ActionResult Index(int id)
        {
            var product = db.Products.Where(x => x.ID == id).FirstOrDefault();
            ViewBag.ListColors = db.Colors.ToList();
            ViewBag.ListSizes = db.Sizes.ToList();
            var query = from c in db.Customers
                        join cm in db.Comments
                        on c.Id equals cm.CustomerId
                        where cm.ProductId == id
                        select new CommentViewModel()
                        {
                            Name = c.Name,
                            CreatedDate = cm.CreatedDate,
                            Content = cm.Content
                        };
            ViewBag.ListComments = query.ToList().OrderByDescending(x => x.CreatedDate).ToList();
            ViewBag.ListProducts = db.Products.Where(x => x.ID != product.ID && x.CategoryId == product.CategoryId).Take(5).ToList();
            return View(product);
        }
        public ActionResult List(int? id, int page = 1, int pageSize = 9, string type="")
        {
            IPagedList<Product> result = null;
            var list = db.Products.ToList();
            if(id != null)
            {
                list = list.Where(x => x.CategoryId == id).ToList();
            }
            if (!String.IsNullOrEmpty(type))
            {
                if(type == "giam")
                {
                    list = list.OrderByDescending(x => x.Price).ToList();
                }
                if(type == "tang")
                {
                    list = list.OrderBy(x => x.Price).ToList();
                }
                if (type == "moi")
                {
                    list = list.OrderByDescending(x => x.CreatedDate).ToList();
                }
            }
            result = list.ToPagedList(page, pageSize);
            ViewBag.Categories = db.Categories.ToList();
            ViewBag.Id = id;
            ViewBag.Type = type;
            ViewBag.Products = list.Where(x => x.ActivePromotion == true).ToList();
            return View(result);
        }
        public ActionResult Count(int pId,int? sId,int? cId)
        {
            var list = db.ProductOptions.Where(x => x.ProductId == pId).ToList();
            if(cId != 0)
            {
                list = list.Where(x => x.ColorId == cId).ToList();
            }
            if(sId != 0)
            {
                list = list.Where(x => x.SizeId == sId).ToList();
            }
            int total = 0;
            foreach(var item in list)
            {
                total = total + item.Count;
            }
            return Json(total, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Search(string tukhoa = "", int page = 1, int pageSize = 9)
        {
            IPagedList<Product> result = null;

            var list = db.Products.ToList();
            if (!String.IsNullOrEmpty(tukhoa))
            {
                list = list.Where(x => x.Name.ToUpper().Contains(tukhoa.ToUpper())).ToList();
            }
            result = list.ToPagedList(page, pageSize);
            ViewBag.Categories = db.Categories.ToList();
            ViewBag.Products = list.Where(x => x.ActivePromotion == true).ToList();
            ViewBag.keyWord = tukhoa;
            return View(result); 
        }
    }
}