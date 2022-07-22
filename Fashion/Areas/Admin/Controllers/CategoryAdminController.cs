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
    public class CategoryAdminController : Controller
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
        public ActionResult Create(Category model)
        {
            try
            {
                Category entity = new Category();
                entity.Name = model.Name;
                entity.Alias = XString.ToAscii(model.Name);
                entity.CreatedDate = DateTime.Now;
                db.Categories.Add(entity);
                db.SaveChanges();
                Notification.set_flash("Thêm danh mục thành công!", "success");
                return RedirectToAction("List");
            }
            catch
            {
                Notification.set_flash("Thêm danh mục thất bại!", "danger");
                throw;
            }
        }
        public ActionResult List()
        {
            var result = db.Categories.OrderByDescending(x => x.ID).ToList();
            return View(result);
        }
        [HttpGet]
        public ActionResult Update(int id)
        {
            var entity = db.Categories.Find(id);
            return View(entity);
        }
        [HttpPost]
        public ActionResult Update(Category category)
        {
            try
            {
                Category entity = db.Categories.Find(category.ID);
                entity.Name = category.Name;
                entity.Alias = XString.ToAscii(category.Name);
                db.SaveChanges();
                Notification.set_flash("Cập nhật danh mục thành công!", "success");
                return RedirectToAction("List");
            }
            catch
            {
                Notification.set_flash("Cập nhật danh mục thất bại!", "danger");
                throw;
            }
        }
        public ActionResult Delete(int id)
        {
            try
            {
                var products = db.Products.Where(x => x.CategoryId == id).ToList();
                if (products.Count() > 0)
                {
                    Notification.set_flash("Không được phép xóa!", "warning");
                    return Json(false, JsonRequestBehavior.AllowGet);
                }
                var model = db.Categories.Where(x => x.ID == id).FirstOrDefault();
                db.Categories.Remove(model);
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