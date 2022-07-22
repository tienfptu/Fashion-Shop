using Fashion.Library;
using Fashion.Models;
using Fashion.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.ModelBinding;
using System.Web.Mvc;
using QRCoder;
using System.IO;
using System.Drawing;

namespace Fashion.Controllers
{
    public class CartController : Controller
    {
        public FSDbContext db = new FSDbContext();

        [HttpPost]
        public ActionResult AddCart(int pid, int qty, int cId, int sId)
        {
            if ((Customer)Session["CUS"] == null)
            {
                return Json(new { result = 4 }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var query = from p in db.Products
                            join op in db.ProductOptions on p.ID equals op.ProductId
                            join c in db.Colors on op.ColorId equals c.ID
                            join s in db.Sizes on op.SizeId equals s.ID
                            where p.ID == pid && c.ID == cId && s.ID == sId
                            select new CartViewModel()
                            {
                                ProductID = pid,
                                Name = p.Name,
                                Quantity = qty,
                                Price = (p.ActivePromotion == true ? p.PromotionPrice : p.Price),
                                Image = p.Image,
                                SizeId = sId,
                                ColorId = cId,
                                ColorName = c.Name,
                                SizeName = s.Name
                            };
                var _p = query.FirstOrDefault();
                var cart = Session["Cart"];
                if (cart == null)
                {
                    var list = new List<CartViewModel>();
                    list.Add(_p);
                    Session["Cart"] = list;
                    return Json(new { result = 1 }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var list = (List<CartViewModel>)cart;

                    if (list.Exists(m => m.ProductID == pid && m.SizeId == sId && m.ColorId == cId))
                    {
                        foreach (var item in list)
                        {
                            if (item.ProductID == pid && item.SizeId == sId && item.ColorId == cId)
                            {
                                item.Quantity += qty;
                                return Json(new { result = 2 }, JsonRequestBehavior.AllowGet);
                            }
                        }
                    }
                    else
                    {
                        list.Add(_p);
                        return Json(new { result = 1 }, JsonRequestBehavior.AllowGet);
                    }
                }
                return Json(new { result = 0 }, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult RemoveAll()
        {
            Session.Remove("Cart");

            return RedirectToAction("ViewCart");
        }
        public ActionResult Checkout()
        {
            if (Session["CUS"] != null && Session["Cart"] != null)
            {
                var user = (Customer)Session["CUS"];
                return View(user);
            }
            else
                return RedirectToAction("ViewCart", "Cart");
        }

        [HttpPost]
        public ActionResult Payment(Customer model)
        {
            var _cart = (CartTotal)Session["CartPrint"];
            var order = new Order();
            if (!String.IsNullOrEmpty(_cart.Code))
            {
                order.Code = _cart.Code;
            }
            order.CustomerName = model.Name;
            order.CustomerId = model.Id;
            order.CustomerEmail = model.Email;
            order.CustomerAddress = model.Address;
            order.CustomerMobile = model.Phone;
            order.CustomerMessage = model.AddressMore;
            order.CreatedBy = model.Id;
            order.CreatedDate = DateTime.Now;
            order.Status = 1;
            order.Total = (float)_cart.Payment;
            db.Orders.Add(order);
            db.SaveChanges();
            foreach (var c in (List<CartViewModel>)Session["Cart"])
            {
                var product = db.ProductOptions.Where(x => x.ProductId == c.ProductID && x.ColorId == c.ColorId && x.SizeId == c.SizeId).FirstOrDefault();
                product.Count = product.Count - c.Quantity;
                db.SaveChanges();
                var orderdetails = new OrderDetail();
                orderdetails.OrderId = order.ID;
                orderdetails.Price = (float)c.Price;
                orderdetails.ProductId = c.ProductID;
                orderdetails.Quantity = c.Quantity;
                orderdetails.ColorId = c.ColorId;
                orderdetails.SizeId = c.SizeId;
                db.OrderDetails.Add(orderdetails);
            }

            db.SaveChanges();
            Session.Remove("Cart");
            Session.Remove("CartPrint");
            return RedirectToAction("Success");

        }
        [HttpGet]
        public ActionResult Update(int pId, int sId, int cId, int count)
        {
            var cart = (List<CartViewModel>)Session["Cart"];
            var item = cart.Where(x => x.ProductID == pId && x.SizeId == sId && x.ColorId == cId).FirstOrDefault();
            item.Quantity = count;
            return Json(true, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult ListCart()
        {
            var cart = (List<Fashion.ViewModel.CartViewModel>)Session["Cart"];
            var result = "";
            if (cart != null)
            {
                float? total = 0;
                foreach (var item in cart)
                {

                    var priceF = string.Format("{0:#,0}", item.Price);
                    total = total + item.Price * item.Quantity;

                    var html = "<div class=\"cart-row\"> <a href=\"/product/index/" + item.ProductID + "\" class=\"img\"><img src=\"" + item.Image + "\" alt=\"image\" class=\"img-responsive\"></a> <div class=\"mt-h\"> <span class=\"mt-h-title\"><a href=\"/product/index/" + item.ProductID + "\">" + item.Name + "</a></span> <span class=\"price\"> " + priceF + " VNĐ</span> <span class=\"mt-h-title\">Số lượng: " + item.Quantity + "</span><span class=\"mt-h-title\">Màu sắc: " + item.ColorName + "</span> <span class=\"mt-h-title\">Kích cỡ: " + item.SizeName + "</span> </div> <a href=\"#\" class=\"close fa fa-times\"></a></div>";
                    result = result + html;
                }
                var totalF = string.Format("{0:#,0}", total);
                var end = "<div class=\"cart-row-total\"> <span class=\"mt-total\">Tổng tiền</span> <span class=\"mt-total-txt\">" + totalF + " VNĐ</span></div><div class=\"cart-btn-row\">  <a href=\"/cart/viewcart\" class=\"btn-type3\">Thanh toán</a></div>";
                result = result + end;
                return Json(new { cart = cart, result = result }, JsonRequestBehavior.AllowGet);
            }
            return Json(null, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ViewCart(string code = "")
        {
            if (Session["CUS"] != null && Session["Cart"] != null)
            {
                var cus = (Customer)Session["CUS"];
                var carttotal = new CartTotal();
                float? _price = 0;
                var cart = (List<Fashion.ViewModel.CartViewModel>)Session["Cart"];
                foreach (var item in cart)
                {
                    _price = _price + item.Price * item.Quantity;
                }
                carttotal.Total = _price;
                carttotal.Payment = _price;
                if (String.IsNullOrEmpty(code))
                {
                    carttotal.Payment = _price;
                }
                else
                {
                    var entity = db.Discounts.Where(x => x.Code == code && x.Status == true).FirstOrDefault();

                    if (entity != null)
                    {

                        var ischeck = entity.CreatedDate.AddDays(entity.Time);
                        var od = db.Orders.Where(x => x.Code == code && x.CustomerId == cus.Id).FirstOrDefault();
                        ViewBag.Error = null;
                        if (ischeck < DateTime.Now)
                        {
                            ViewBag.Error = "Mã giảm giá đã hết hạn";
                        }
                        else if (od != null)
                        {
                            ViewBag.Error = "Mã giảm giá đã sử dụng ";
                        }
                        else
                        {
                            carttotal.Value = entity.Value.ToString();
                            carttotal.Code = code;
                            carttotal.Payment = _price - ((_price * entity.Value) / 100);
                        }
                    }
                    else if (entity == null)
                    {
                        ViewBag.Error = "Mã giảm giá không hợp lệ";
                    }
                }
                Session["CartPrint"] = carttotal;
                return View(cart);
            }
            else
                return RedirectToAction("Index", "Home");

        }
        public ActionResult Success()
        {

            using (MemoryStream ms = new MemoryStream())
            {
                String qrcode = "localhost:44381/Customer/Bill";
                QRCodeGenerator qRCodeGeneator = new QRCodeGenerator();
                QRCodeData qrCodeData = qRCodeGeneator.CreateQrCode(qrcode, QRCodeGenerator.ECCLevel.Q);
                System.Web.UI.WebControls.Image imgBarCode = new System.Web.UI.WebControls.Image();
                QRCode qrCode = new QRCode(qrCodeData);

                using (Bitmap bitmap = qrCode.GetGraphic(20))
                {

                    bitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                    byte[] byteImage = ms.ToArray();
                    ViewBag.imgBarCode = "data:image/png;charset=utf-8;base64," + Convert.ToBase64String(byteImage);

                }
            }
            return View();
        }



        public ActionResult RemoveItem(int pId, int sId, int cId)
        {
            var cart = (List<Fashion.ViewModel.CartViewModel>)Session["Cart"];
            var item = cart.Where(x => x.ProductID == pId && x.SizeId == sId && x.ColorId == cId).FirstOrDefault();
            cart.Remove(item);
            return RedirectToAction("ViewCart");
        }
    }
}