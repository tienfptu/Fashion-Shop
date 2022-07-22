using Fashion.Models;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Fashion
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new SessionFilter());

        }
    }
    public class SessionFilter : ActionFilterAttribute
    {

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {

            if (HttpContext.Current.User.Identity.IsAuthenticated && HttpContext.Current.Session["UserID"] == null)
            {
                FSDbContext db = new FSDbContext();
                bool Exist = db.Users.Any(e => e.Username == HttpContext.Current.User.Identity.Name);
                if (Exist)
                {
                    var user = db.Users.Where(e => e.Username == HttpContext.Current.User.Identity.Name).First();
                    HttpContext.Current.Session["USER"] = user;
                }
            }
            base.OnActionExecuting(filterContext);
        }

    }

}
