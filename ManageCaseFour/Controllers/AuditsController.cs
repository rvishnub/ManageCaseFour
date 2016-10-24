using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ManageCaseFour.Models;
using static ManageCaseFour.Models.Audit;

namespace ManageCaseFour.Controllers
{
    [Authorize(Roles = "Admin")]
    [Audit]
    public class AuditsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private Audit.AuditingContext dbA = new Audit.AuditingContext();

        // GET: Audits
        //public ActionResult Index()
        //{
        //    return View(db.Audits.ToList());
        //}
        public ActionResult Index()
        {
            var audits = new AuditingContext().AuditRecords.ToList().OrderBy(x => x.TimeAccessed).ToList();
            return View(audits);
        }

        // GET: Audits/Details/5
        [Audit]
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Audit audit = dbA.AuditRecords.Find(id);
            if (audit == null)
            {
                return HttpNotFound();
            }
            return View(audit);
        }

        // GET: Audits/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Audits/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "AuditID,UserName,IPAddress,AreaAccessed,Timestamp")] Audit audit)
        {
            return RedirectToAction("Index");
        }

        // GET: Audits/Edit/5
        public ActionResult Edit(Guid? id)
        {
            return RedirectToAction("Index");
        }

        // POST: Audits/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "AuditID,UserName,IPAddress,AreaAccessed,Timestamp")] Audit audit)
        {
            return RedirectToAction("Index");
        }

        // GET: Audits/Delete/5
        public ActionResult Delete(Guid? id)
        {
            return RedirectToAction("Index");
        }

        // POST: Audits/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        public class AuditAttribute : ActionFilterAttribute
        {
            public override void OnActionExecuting(ActionExecutingContext filterContext)
            {
                var request = filterContext.HttpContext.Request;
                Audit audit = new Audit()
                {
                    AuditID = Guid.NewGuid(),
                    UserName = (request.IsAuthenticated) ? filterContext.HttpContext.User.Identity.Name : "Anonymous",
                    IPAddress = request.ServerVariables["HTTP_X_FORWARDED_FOR"] ?? request.UserHostAddress,
                    AreaAccessed = request.RawUrl,
                    TimeAccessed = DateTime.Now,
                };

                Audit.AuditingContext context = new Audit.AuditingContext();
                context.AuditRecords.Add(audit);
                context.SaveChanges();

                base.OnActionExecuting(filterContext);
            }


        }
    }
}
