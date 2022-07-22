using Fashion.Dtos;
using Fashion.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Fashion.Controllers
{
    public class CommentController : Controller
    {
        private FSDbContext db = new FSDbContext();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Create(Comment model)
        {
            var user = (Customer)Session["CUS"];
            var entity = new Comment();
            entity.Content = model.Content;
            entity.CreatedDate = DateTime.Now;
            entity.ProductId = model.ProductId;
            entity.CustomerId = user.Id;
            db.Comments.Add(entity);
            db.SaveChanges();
            int id = model.ProductId;
            return RedirectToAction("Index","Product", new { id });
        }
    }
}