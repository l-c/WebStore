using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebStore.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            // Cookie Test #Ignore#

            ViewBag.Message = "Welcome to ASP.NET MVC!";

            if(Request.Cookies["CART"] == null)
            {
                var hp = new HttpCookie("CART");
                hp["VISITS"] = "1";
                Response.Cookies.Add(hp);
                ViewBag.Data = "First Time Visitor";

            }
            else
            {
                var hp = Request.Cookies["CART"];
                var counter = Convert.ToInt32(hp["VISITS"]) + 1;
                hp["VISITS"] = Convert.ToString(counter);
                Response.AppendCookie(hp);
                ViewBag.Data = String.Format("Number of visits: {0}",counter);

            }
            return View();
        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Data = "Contact Me";
            return View();
        }
    }
}
