using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ManageCaseFour.Models;

namespace ManageCaseFour.Controllers
{
    [Authorize(Roles = "Admin, Manager")]
    public class CaseController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Case
        public ActionResult Index()
        {
            List<Case> CaseList = db.Case.ToList();
            return View(CaseList);
        }

        // GET: Case/Details/5
        public ActionResult Details(string searchValue, string param)
        {
            NewCaseViewModel nCVModel = new NewCaseViewModel();
            List<Case> CaseLoad = new List<Case>();
            if (param == "Principal")
            {
                var principal = db.Principal.Select(v => v).Where(y => y.principalCode == searchValue).First();
                var CaseJunctionList = db.PrincipalCaseJunction.Select(x => x).Where(y => y.principalId == principal.principalId).ToList();
            }
            else if (param == "Employee")
            {

                var employee = db.Users.Select(v => v).Where(y => y.UserName == searchValue).First();
                var CaseJunctionList = db.UserCaseJunction.Select(x => x).Where(y => y.Id == employee.Id).ToList();
                for (int i = 0; i < CaseJunctionList.Count(); i++)
                {
                    var selectedCaseId = CaseJunctionList[i];
                    var selectedCase = db.Case.Select(x => x).Where(y => y.caseId == selectedCaseId.caseId).First();
                    CaseLoad.Add(selectedCase);
                }
            }
            nCVModel.CaseLoad = CaseLoad;
            return View(nCVModel);
        }

        // GET: Case/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Case/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "caseId,caseName,caseNumber")] Case @case)
        {
            if (ModelState.IsValid)
            {
                db.Case.Add(@case);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(@case);
        }

        // GET: Case/Edit/5
        public ActionResult Edit(int? id)
        {
            NewCaseViewModel nCVModel = new NewCaseViewModel();
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Case selectedCase = db.Case.Find(id);
            if (selectedCase == null)
            {
                return HttpNotFound();
            }
            var caseJunc = db.PrincipalCaseJunction.Select(x => x).Where(y => y.caseId == selectedCase.caseId).First();
            List<Principal> CaseTeam = db.Principal.Select(x => x).Where(y => y.principalId == caseJunc.principalId).ToList();

            var userCaseJunc = db.UserCaseJunction.Select(x => x).Where(y => y.caseId == selectedCase.caseId).First();
            var CaseStaffMembers = db.Users.Select(x => x).Where(y => y.Id == userCaseJunc.Id).ToList();

            InternalCaseNumber intCaseNumber = db.InternalCaseNumber.Select(x => x).Where(y => y.caseId == selectedCase.caseId).First();

            nCVModel.newCase = selectedCase;
            nCVModel.CaseTeam = CaseTeam;
            nCVModel.CaseStaffMembers = CaseStaffMembers;
            nCVModel.intCaseNumber = intCaseNumber;
            return View(nCVModel);
        }

        // POST: Case/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "caseId, caseNumber, CaseTeam, CaseStaffMembers, intCaseNumber")] NewCaseViewModel nCVModel)
        {
            if (ModelState.IsValid)
            {
                db.Entry(nCVModel).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(nCVModel);
        }

        // GET: Case/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Case @case = db.Case.Find(id);
            if (@case == null)
            {
                return HttpNotFound();
            }
            return View(@case);
        }

        // POST: Case/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Case @case = db.Case.Find(id);
            db.Case.Remove(@case);
            db.SaveChanges();
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
    }
}
