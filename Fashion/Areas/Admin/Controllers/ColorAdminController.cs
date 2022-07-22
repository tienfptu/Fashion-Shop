using Fashion.Library;
using Fashion.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Fashion.Areas.Admin.Controllers
{
    [Authorize]
    public class ColorAdminController : Controller
    {
        private FSDbContext db = new FSDbContext();
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(Color model)
        {
            try
            {
                Color entity = new Color();
                entity.Name = model.Name;
                db.Colors.Add(entity);
                db.SaveChanges();
                Notification.set_flash("Thêm màu thành công!", "success");
                return RedirectToAction("List");
            }
            catch
            {
                Notification.set_flash("Thêm màu thất bại!", "danger");
                throw;
            }
        }
        public ActionResult List()
        {
            var result = db.Colors.OrderByDescending(x => x.ID).ToList();
            return View(result);
        }
        [HttpGet]
        public ActionResult Update(int id)
        {
            var entity = db.Colors.Find(id);
            return View(entity);
        }
        [HttpPost]
        public ActionResult Update(Color Color)
        {
            try
            {
                Color entity = db.Colors.Find(Color.ID);
                entity.Name = Color.Name;
                db.SaveChanges();
                Notification.set_flash("Cập nhật màu thành công!", "success");
                return RedirectToAction("List");
            }
            catch
            {
                Notification.set_flash("Cập nhật màu thất bại!", "danger");
                throw;
            }
        }
        public ActionResult Delete(int id)
        {
            try
            {
                var products = db.ProductOptions.Where(x => x.ColorId == id).ToList();
                if (products.Count() > 0)
                {
                    Notification.set_flash("Không được phép xóa!", "warning");
                    return Json(false, JsonRequestBehavior.AllowGet);
                }
                var model = db.Colors.Where(x => x.ID == id).FirstOrDefault();
                db.Colors.Remove(model);
                db.SaveChanges();
                Notification.set_flash("Xóa thành công!", "success");
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                Notification.set_flash("Xóa sản phẩm thất bại!", "danger");
                throw;
            }
        }
    }
}