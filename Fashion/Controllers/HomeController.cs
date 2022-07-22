using Fashion.Models;
using Fashion.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Fashion.Controllers
{
    public class HomeController : Controller
    {
        private FSDbContext db = new FSDbContext();
        public ActionResult Index()
        {
            var screen = new ProductViewModel();
            screen.ProductHots = db.Products.Where(x => x.Status == true && x.HotFlag == true).OrderByDescending(x => x.ID).ToList();
            screen.ProductNews = db.Products.Where(x => x.Status == true).OrderByDescending(x => x.ID).ToList();
            screen.ProductPromotions = db.Products.Where(x => x.Status == true && x.ActivePromotion == true).OrderByDescending(x => x.ID).ToList();
            screen.ProductPriceTop = db.Products.Where(x => x.Status == true).OrderByDescending(x => x.Price).ToList();
            screen.ProductPriceBot = db.Products.Where(x => x.Status == true).OrderBy(x => x.Price).ToList();
            return View(screen);
        }
        public ActionResult _Header()
        {
            var list = db.Categories.ToList();
            return PartialView(list);
        }
        public ActionResult _Cart()
        {
            return PartialView();
        }


    }
}