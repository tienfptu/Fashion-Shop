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
    public class ProductOptionAdminController : Controller
    {
        private FSDbContext db = new FSDbContext();
        public ActionResult Index(int id)
        {
            ViewBag.ListColors = new SelectList(db.Colors.ToList(), "ID", "Name", 0);
            ViewBag.ListSizes = new SelectList(db.Sizes.ToList(), "ID", "Name", 0);
            ViewBag.Product = db.Products.Find(id);
            ViewBag.Option = db.ProductOptions.Where(x=>x.ProductId == id).OrderByDescending(x=>x.Id).ToList();
            return View();
        }
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(ProductOption model)
        {
            try
            {
                ProductOption entity = new ProductOption();
                entity.ColorId = model.ColorId;
                entity.SizeId = model.SizeId;
                entity.Count = model.Count;
                entity.ProductId = model.ProductId;
                entity.CreatedDate = DateTime.Now;
                db.ProductOptions.Add(entity);
                db.SaveChanges();
                int id = model.ProductId;
                Notification.set_flash("Thêm thông tin sản phẩm thành công!", "success");
                return RedirectToAction("Index",new { id });
            }
            catch
            {
                Notification.set_flash("Thêm thông tin sản phẩm thất bại!", "danger");
                throw;
            }
        }
        [HttpGet]
        public ActionResult Update(int id)
        {
            var entity = db.ProductOptions.Find(id);
            return Json(entity, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult Update(ProductOption ProductOption)
        {
            try
            {
                ProductOption entity = db.ProductOptions.Find(ProductOption.Id);
                entity.ColorId = ProductOption.ColorId;
                entity.SizeId = ProductOption.SizeId;
                entity.ProductId = ProductOption.ProductId;
                entity.Count = ProductOption.Count;
                db.SaveChanges();
                int id = ProductOption.ProductId;
                Notification.set_flash("Cập nhật thông tin sản phẩm thành công!", "success");
                return RedirectToAction("Index", new { id });
            }
            catch
            {
                Notification.set_flash("Cập nhật thông tin sản phẩm thất bại!", "danger");
                throw;
            }
        }
        public ActionResult Delete(int id)
        {
            try
            {
                var model = db.ProductOptions.Where(x => x.Id == id).FirstOrDefault();
                db.ProductOptions.Remove(model);
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