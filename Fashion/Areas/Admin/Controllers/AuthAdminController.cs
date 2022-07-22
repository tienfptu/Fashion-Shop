using Fashion.Dtos;
using Fashion.Library;
using Fashion.Models;
using Fashion.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Fashion.Areas.Admin.Controllers
{
    public class AuthAdminController : Controller
    {
        private FSDbContext db = new FSDbContext();
        public ActionResult Index()
        {
            return RedirectToAction("Login");
        }
        public ActionResult Login()
        {
            if (Request.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            Session.Clear();
            return View();
        }
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            Session.Clear();
            return RedirectToAction("Index", "AdminAdmin");
        }
        public void setCookie(string name, string username ="")
        {
            var authTicket = new FormsAuthenticationTicket(
                               1,
                               name,
                               DateTime.Now,
                               DateTime.Now.AddMinutes(120),
                               false,
                               username
                               );

            string encryptedTicket = FormsAuthentication.Encrypt(authTicket);

            var authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
            Response.Cookies.Add(authCookie);
        }

        [HttpPost]
        public ActionResult Login(LoginViewModel model, string ReturnUrl)
        {

            if (ModelState.IsValid)
            {
                var exist = db.Users.Any(e => e.Username.Equals(model.Username));
                if (exist)
                {
                    var user = db.Users.Where(e => e.Username.Equals(model.Username)).First();

                    if (user != null)
                    {
                        if (user.Password == XString.ToMD5(model.Password) && user.Status == true )
                        {
                            setCookie(user.Name, user.Username);
                            var userSession = new UserSession();
                            userSession.Id = user.Id;
                            userSession.Name = user.Name;
                            userSession.Username = user.Username;
                            userSession.Image = user.Image;
                            Session.Add("USER", userSession);
                            if (ReturnUrl != null)
                                return Redirect(ReturnUrl);
                            return RedirectToAction("Index", "AdminAdmin");
                        }
                        ModelState.AddModelError("", "Sai tài khoản hoặc mật khẩu!");
                        return View();

                    }
     
                }

            }
            return View();
        }
    }
}