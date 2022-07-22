using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Fashion.Models;
using Fashion.Library;
namespace Fashion.Controllers
{
    public class QuenMKController : Controller
    {
        // GET: QuenMK
        private FSDbContext db = new FSDbContext();

        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Index(QuenMK cus)
        {
            Customer rs = db.Customers.SingleOrDefault(m => m.Username == cus.username);
            if (rs != null)
            {
                Random ran = new Random();
                string newPass = ran.Next(10000000, 99999999).ToString();
                rs.Password = XString.ToMD5(newPass);
                db.SaveChanges();
                string content = System.IO.File.ReadAllText(Server.MapPath("~/Views/QuenMk/QuenMK.html"));
                content = content.Replace("{{newpass}}", newPass);
                new Common.MailHelper().sendMail(rs.Email, "Mật khẩu mới", content);
                // ModelState.AddModelError("", "Mật khẩu mới của bạn đã được gửi qua mail đã đăng ký, vui lòng kiểm tra");
                return RedirectToAction("Success", "QuenMK");

            }
            else
            {
                ModelState.AddModelError("", "Tài khoản không tồn tại");
            }
            return View();
        }

        public ActionResult Success()
        {
            return View();
        }

    }
}