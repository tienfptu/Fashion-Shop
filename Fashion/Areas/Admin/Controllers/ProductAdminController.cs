using Fashion.Library;
using Fashion.Models;
using QRCoder;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Fashion.Areas.Admin.Controllers
{
    [Authorize]
    public class ProductAdminController : Controller
    {
        private FSDbContext db = new FSDbContext();
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult Create()
        {
            ViewBag.ListCategory = new SelectList(db.Categories.ToList(), "ID", "Name", 0);
            return View();
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(Product product)
        {
            try
            {
                Product entity = new Product();
                entity.Name = product.Name;
                entity.Alias = XString.ToAscii(product.Name);
                entity.CategoryId = product.CategoryId;
                entity.Image = product.Image;
                entity.OriginalPrice = product.OriginalPrice;
                entity.Price = product.Price;
                entity.PromotionPrice = product.PromotionPrice;
                entity.Description = product.Description;
                entity.Content = product.Content;
                entity.ViewCount = 0;
                entity.Status = product.Status;
                entity.CreatedDate = DateTime.Now;
                QRCodeGenerator qrGenerator = new QRCodeGenerator();
                QRCodeData qrCodeData  = qrGenerator.CreateQrCode(entity.Alias, QRCodeGenerator.ECCLevel.Q);
                System.Web.UI.WebControls.Image imgBarCode = new System.Web.UI.WebControls.Image();
                QRCode qrCode = new QRCode(qrCodeData);
                imgBarCode.Height = 150;
                imgBarCode.Width = 150;
                using (Bitmap bitMap = qrCode.GetGraphic(20))
                {
                    using (MemoryStream ms = new MemoryStream())
                    {
                        bitMap.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                        byte[] byteImage = ms.ToArray();
                        imgBarCode.ImageUrl = "data:image/png;charset=utf-8;base64," + Convert.ToBase64String(byteImage);
                    }
                }
                entity.QrCode = imgBarCode.ImageUrl;
               
                    db.Products.Add(entity);
                    db.SaveChanges();
                    Notification.set_flash("Thêm mới sản phẩm thành công!", "success");
                    return RedirectToAction("List");
                 
                
            }
            catch
            {
                Notification.set_flash("Thêm mới sản phẩm thất bại!", "danger");
                throw;
            }
        }
        public ActionResult List()
        {
            var result = db.Products.OrderByDescending(x => x.ID).ToList();
            return View(result);
        }
        [HttpGet]
        public ActionResult Update(int Id)
        {
            var model = db.Products.Find(Id);
            ViewBag.ListCategory = new SelectList(db.Categories.ToList(), "ID", "Name", 0);
            return View(model);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Update(Product product)
        {
            try
            {
                Product entity = db.Products.Find(product.ID);
                entity.Name = product.Name;
                entity.Alias = XString.ToAscii(product.Name);
                entity.CategoryId = product.CategoryId;
                entity.Image = product.Image;
                entity.OriginalPrice = product.OriginalPrice;
                entity.Price = product.Price;
                entity.PromotionPrice = product.PromotionPrice;
                entity.Description = product.Description;
                entity.Content = product.Content;
                entity.Status = product.Status;
                QRCodeGenerator qrGenerator = new QRCodeGenerator();
                QRCodeData qrCodeData = qrGenerator.CreateQrCode(entity.Alias, QRCodeGenerator.ECCLevel.Q);
                System.Web.UI.WebControls.Image imgBarCode = new System.Web.UI.WebControls.Image();
                QRCode qrCode = new QRCode(qrCodeData);
                imgBarCode.Height = 150;
                imgBarCode.Width = 150;
                using (Bitmap bitMap = qrCode.GetGraphic(20))
                {
                    using (MemoryStream ms = new MemoryStream())
                    {
                        bitMap.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                        byte[] byteImage = ms.ToArray();
                        imgBarCode.ImageUrl = "data:image/png;charset=utf-8;base64," + Convert.ToBase64String(byteImage);
                    }
                }
                entity.QrCode = imgBarCode.ImageUrl;
                db.SaveChanges();
                Notification.set_flash("Cập nhật sản phẩm thành công!", "success");
                return RedirectToAction("List");
            }
            catch
            {
                Notification.set_flash("Cập nhật sản phẩm thất bại!", "danger");
                throw;
            }
        }
        public ActionResult Delete(int Id)
        {
            try
            {
                var entity = db.Products.Find(Id);
                var listOption = db.ProductOptions.Where(x => x.ProductId == entity.ID).ToList();
                foreach(var option in listOption)
                {
                    db.ProductOptions.Remove(option);
                }
                db.Products.Remove(entity);
                db.SaveChanges();
                Notification.set_flash("Xóa sản phẩm thành công!", "success");
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                Notification.set_flash("Xóa sản phẩm thất bại!", "danger");
                throw;
            }
        }
        public JsonResult ChangeShow(int id, bool status)
        {
            var model = db.Products.Where(x => x.ID == id).FirstOrDefault();
            model.Status = status;
            db.SaveChanges();
            Notification.set_flash("Cập nhật thành công!", "success");
            return Json(true, JsonRequestBehavior.AllowGet);
        }
        public JsonResult ChangeHot(int id, bool status)
        {
            var model = db.Products.Where(x => x.ID == id).FirstOrDefault();
            model.HotFlag = status;
            db.SaveChanges();
            Notification.set_flash("Cập nhật thành công!", "success");
            return Json(true, JsonRequestBehavior.AllowGet);
        }
        public JsonResult ChangePromotion(int id, bool status)
        {
            var model = db.Products.Where(x => x.ID == id).FirstOrDefault();
            model.ActivePromotion = status;
            db.SaveChanges();
            Notification.set_flash("Cập nhật thành công!", "success");
            return Json(true, JsonRequestBehavior.AllowGet);
        }
    }
}