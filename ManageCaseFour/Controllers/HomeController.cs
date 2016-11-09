using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ManageCaseFour.Models;

namespace ManageCaseFour.Controllers
{
    public class HomeController : Controller
    {

        ApplicationDbContext db = new ApplicationDbContext();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        [Authorize]
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";
            return View();
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Admin()
        {
            string Uri = Url.HttpRouteUrl("Default", new { controller = "admin", });
            ViewBag.Url = new Uri(Request.Url, Uri).AbsoluteUri.ToString();

            List<Case> usersCasesList = db.Case.Include(x => x.Users).ToList();
            thisCase.usersCases = usersCasesList.ToArray();
            return View(thisCase);
        }

        public ActionResult Manager()
        {
            string Uri = Url.HttpRouteUrl("Default", new { controller = "manager", });
            ViewBag.Url = new Uri(Request.Url, Uri).AbsoluteUri.ToString();
            return View();

        }
    }
}