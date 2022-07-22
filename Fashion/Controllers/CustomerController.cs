using Fashion.Library;
using Fashion.Models;
using Fashion.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Fashion.Controllers
{
    public class CustomerController : Controller
    {
        private FSDbContext db = new FSDbContext();
        public ActionResult Index()
        {
            var cus = (Customer)Session["CUS"];
            if (cus != null)
            {
                var entity = db.Customers.Where(x => x.Id == cus.Id).FirstOrDefault();
                return View(entity);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        [HttpPost]
        public ActionResult Login(Customer model)
        {
            model.Password = XString.ToMD5(model.Password);
            var cus = db.Customers.Where(x => x.Username == model.Username && x.Password == model.Password).FirstOrDefault();
            if (cus!=null)
            {
                Session["CUS"] = cus;
                Session["Cart"] = null;
                return Json(1, JsonRequestBehavior.AllowGet);

            }


            return Json(0, JsonRequestBehavior.AllowGet);

        }
        public int checkAcount(string username, string mail, string phone)
        {
            var admin = db.Users.Count(m => m.Username == username);
            var usern = db.Customers.Count(m => m.Username == username);
            var gmail = db.Customers.Count(m => m.Email == mail);
            var phoneNum = db.Customers.Count(m => m.Phone == phone);
            if (admin > 0 || usern > 0)
                return 0;

            if (gmail > 0)
                return 2;
            if (phoneNum > 0)
                return 3;
            return 1;
        }
        [HttpPost]
        public ActionResult Register(Customer model, string RePass)
        {
            Customer entity = new Customer();
            var rs = checkAcount(model.Username, model.Email, model.Phone);
            if(model.Password == RePass)
            {
                if (rs == 1)
                {
                    entity.Username = model.Username;
                    entity.Password = XString.ToMD5(model.Password);
                    entity.Address = model.Address;
                    entity.Phone = model.Phone;
                    entity.CreatedDate = DateTime.Now;
                    entity.Name = model.Name;
                    entity.Email = model.Email;

                    db.Customers.Add(entity);
                    db.SaveChanges();
                    return Json(1, JsonRequestBehavior.AllowGet);

                }
                if (rs == 0)
                    return Json(0, JsonRequestBehavior.AllowGet);

                if (rs == 2)
                    return Json(2, JsonRequestBehavior.AllowGet);
                else
                    return Json(3, JsonRequestBehavior.AllowGet);

            }
            
            return Json(4, JsonRequestBehavior.AllowGet);

        }
        public ActionResult ChangePass()
        {
            return View();
        }
        public ActionResult ListOrder()
        {
            var cus = (Customer)Session["CUS"];
            if (cus != null)
            {
                var entity = db.Orders.Where(x => x.CustomerId == cus.Id).OrderByDescending(y => y.CreatedDate).ToList();
                return View(entity);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        public ActionResult Bill()
        {
            var cus = (Customer)Session["CUS"];
            if (cus != null)
            {
                var entity = db.Orders.Where(x => x.CustomerId == cus.Id).OrderByDescending(y => y.CreatedDate).FirstOrDefault();
                return View(entity);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        public ActionResult Logout()
        {
            Session["CUS"] = null;
            Session["Cart"] = null;
            return RedirectToAction("Index", "Home");
        }
        public ActionResult Cancel(int Id)
        {
            var ressult = db.Orders.Where(x => x.ID == Id).FirstOrDefault();
            ressult.Status = 5;
            db.SaveChanges();
            return RedirectToAction("ListOrder");
        }
        public ActionResult Update(Customer model, string Repass)
        {

            var entity = db.Customers.Find(model.Id);
            entity.Username = model.Username;
            if( Repass == model.Password)
            {
                if (entity.Password != model.Password)
                {
                    entity.Password = XString.ToMD5(model.Password);
                }

            }


            entity.Address = model.Address;
            entity.Phone = model.Phone;
            entity.Name = model.Name;
            entity.Email = model.Email;
            db.SaveChanges();
            return RedirectToAction("Index", "Customer");
        }

    }
}