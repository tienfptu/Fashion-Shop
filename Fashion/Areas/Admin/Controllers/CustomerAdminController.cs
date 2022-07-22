using Fashion.Models;
using Fashion.ViewModel;
using System.Linq;
using System.Web.Mvc;
using Fashion.Library;

namespace Fashion.Areas.Admin.Controllers
{
    [Authorize]
    public class CustomerAdminController : Controller
    {
        private FSDbContext db = new FSDbContext();
        public ActionResult Index()
        {
            var query = from od in db.Orders
                        join c in db.Customers
                        on od.CustomerId equals c.Id
                        where od.Status == 4
                        group od by new { c.Id, c.Name, c.Email, c.Phone, c.Username, c.Address } into groupc
                        orderby groupc.Key.Id descending
                        select new CustomerViewModel()
                        {
                            Id = groupc.Key.Id,
                            Address = groupc.Key.Address,
                            Name = groupc.Key.Name,
                            Email = groupc.Key.Email,
                            Phone = groupc.Key.Phone,
                            Username = groupc.Key.Username,
                            Count = groupc.Sum(c => c.Total)
                        };
            var result = query.ToList();
            return View(result);
        }
        public ActionResult List()
        {
            var query = from od in db.Orders
                        join c in db.Customers
                        on od.CustomerId equals c.Id
                        group od by new { c.Id, c.Name, c.Email, c.Phone, c.Username, c.Address } into groupc
                        orderby groupc.Key.Id descending
                        select new CustomerViewModel()
                        {
                            Id = groupc.Key.Id,
                            Name = groupc.Key.Name,
                            Username = groupc.Key.Username,
                            Address = groupc.Key.Address,
                            choxacnhanh = groupc.Count(x=>x.Status == 1),
                            xacnhan = groupc.Count(x => x.Status == 2),
                            danggiao = groupc.Count(x => x.Status == 3),
                            dathanhtoan = groupc.Count(x => x.Status == 4),
                            dahuy = groupc.Count(x => x.Status == 5),
                        };
            var result = query.ToList();
            return View(result);
        }
        public ActionResult Comment()
        {
            var query = from od in db.Comments
                        join c in db.Customers
                        on od.CustomerId equals c.Id
                        join p in db.Products
                        on od.ProductId equals p.ID
                        group od by new {od.Id, c.Name, od.CreatedDate, od.Content, Pname = p.Name } into groupc
                        orderby groupc.Key.Name descending
                        select new CommentViewModel()
                        {
                            id=groupc.Key.Id,
                            Name = groupc.Key.Name,
                            NameProduct = groupc.Key.Pname,
                            Content = groupc.Key.Content,
                            CreatedDate = groupc.Key.CreatedDate,
                        };
            var rs = query.ToList();
            return View(rs);
        }
        public ActionResult delete(int id)
        {
            var rs = db.Comments.Where(x => x.Id == id).FirstOrDefault();
            db.Comments.Remove(rs);
            db.SaveChanges();
            Notification.set_flash("Xóa nhận xét thành công!", "success");
            return Json(1, JsonRequestBehavior.AllowGet);

        }

    }
}