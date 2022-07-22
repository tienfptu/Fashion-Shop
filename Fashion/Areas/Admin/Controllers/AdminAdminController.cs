using Dapper;
using Fashion.Models;
using Fashion.ViewModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Fashion.Areas.Admin.Controllers
{
    public class AdminAdminController : Controller
    {
        private FSDbContext db = new FSDbContext();
        public ActionResult Index()
        {
            if (Request.IsAuthenticated)
            {
                ViewBag.TotalOrder = db.Orders.ToList().Count();
                ViewBag.TotalUser = db.Users.ToList().Count();
                ViewBag.TotalCus = db.Customers.ToList().Count();
                ViewBag.TotalPro = db.Products.ToList().Count();
                return View();
            }
            return RedirectToAction("Login", "AuthAdmin");
        }
        [HttpPost]
        public ActionResult Filter(SearchChart model)
        {
            var sql = @"select CONVERT(char(10), o.CreatedDate,126) as Date, CONVERT(char(10), o.CreatedDate,126) as Time, SUM(od.Quantity * p.OriginalPrice) as _Order,SUM(o.Total) as Sales,sum(od.Quantity) as Quantity
                            from dbo.Products as p, dbo.OrderDetails as od, dbo.Orders as o
                            where p.ID = od.ProductId and od.OrderId = o.ID and o.Status = 4
                            group by CONVERT(char(10), o.CreatedDate,126)";
            using (SqlConnection conn = new SqlConnection(@"Data Source=DESKTOP-LGTQKE0;Initial Catalog=FS;Integrated Security=True;"))
            {
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }
                var result = conn.Query<ChartViewModel>(sql, null, null, true, 120, CommandType.Text);

                var _start = DateTime.Parse(model.Start);
                var _end = DateTime.Parse(model.End);

                result = result.Where(x => x.Date < _end && x.Date >= _start).ToList();
                
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }
    }
    
}