using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ManageCaseFour.Models;
using static ManageCaseFour.Controllers.AuditsController;

namespace ManageCaseFour.Controllers
{
    [Authorize]
    public class RecordsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Records
        public ActionResult Index()
        {
            return View(db.Record.ToList());
        }

        // GET: Records/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Record record = db.Record.Find(id);
            if (record == null)
            {
                return HttpNotFound();
            }
            return View(record);
        }

        // GET: Records/Create

        [Audit]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Records/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "recordId,internalCaseId,InternalCaseNumber,sourceId,DocumentSource,departmentId,Department,documentId,DocumentType,facilityId,Facility,recordReferenceNumber,pageNumber,recordEntryDate,provider,memo,serviceDate,noteSubjective,history,noteObjective,noteAssessment,notePlan,medications,age,DOB,allergies,vitalSigns,diagnosis,fileContent")] Record record)
        {
            if (ModelState.IsValid)
            {
                db.Record.Add(record);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(record);
        }

        // GET: Records/Edit/5
        [Audit]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Record record = db.Record.Find(id);
            if (record == null)
            {
                return HttpNotFound();
            }
            return View(record);
        }

        // POST: Records/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "recordId,internalCaseId,InternalCaseNumber,sourceId,DocumentSource,departmentId,Department,documentId,DocumentType,facilityId,Facility,recordReferenceNumber,pageNumber,recordEntryDate,provider,memo,serviceDate,noteSubjective,history,noteObjective,noteAssessment,notePlan,medications,age,DOB,allergies,vitalSigns,diagnosis,fileContent")] Record record)
        {
            if (ModelState.IsValid)
            {
                db.Entry(record).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(record);
        }

        // GET: Records/Delete/5
        [Audit]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Record record = db.Record.Find(id);
            if (record == null)
            {
                return HttpNotFound();
            }
            return View(record);
        }

        // POST: Records/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Record record = db.Record.Find(id);
            db.Record.Remove(record);
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

        public ActionResult Search(string searchTerm)
        {
            List<Record> recordResultList = SearchRecords(searchTerm);
            return View(recordResultList);
        }

        public List<Record> SearchRecords(string searchTerm)
        {
            List<Record> recordResultList = new List<Record>();
            Record record = new Record();
            recordResultList = record.SearchFileContent(searchTerm);
            return recordResultList;
        }
    }
}
