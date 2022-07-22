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
    public class SizeAdminController : Controller
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
        public ActionResult Create(Size model)
        {
            try
            {
                Size entity = new Size();
                entity.Name = model.Name;
                db.Sizes.Add(entity);
                db.SaveChanges();
                Notification.set_flash("Thêm size thành công!", "success");
                return RedirectToAction("List");
            }
            catch
            {
                Notification.set_flash("Thêm size thất bại!", "danger");
                throw;
            }
        }
        public ActionResult List()
        {
            var result = db.Sizes.OrderByDescending(x => x.ID).ToList();
            return View(result);
        }
        [HttpGet]
        public ActionResult Update(int id)
        {
            var entity = db.Sizes.Find(id);
            return View(entity);
        }
        [HttpPost]
        public ActionResult Update(Size Size)
        {
            try
            {
                Size entity = db.Sizes.Find(Size.ID);
                entity.Name = Size.Name;
                db.SaveChanges();
                Notification.set_flash("Cập nhật size thành công!", "success");
                return RedirectToAction("List");
            }
            catch
            {
                Notification.set_flash("Cập nhật size thất bại!", "danger");
                throw;
            }
        }
        public ActionResult Delete(int id)
        {
            try
            {
                var products = db.ProductOptions.Where(x => x.SizeId == id).ToList();
                if (products.Count() > 0)
                {
                    Notification.set_flash("Không được phép xóa!", "warning");
                    return Json(false, JsonRequestBehavior.AllowGet);
                }
                var model = db.Sizes.Where(x => x.ID == id).FirstOrDefault();
                db.Sizes.Remove(model);
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