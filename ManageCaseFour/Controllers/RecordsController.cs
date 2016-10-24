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
using System.Text;
using System.Xml;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Globalization;
using System.Security.Claims;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System.Xml.Linq;

namespace ManageCaseFour.Controllers
{
    [Authorize(Roles = "Employee, Manager")]
    public class RecordsController : Controller
    {

        private ApplicationDbContext db = new ApplicationDbContext();
        OCRViewModel oVModel;

        // GET: Records
        public ActionResult Index()
        {
            oVModel = new OCRViewModel();
            List<OCR> ocrList = db.OCR.ToList();
            OCRViewModel[] oVModelList = oVModel.GetOCRViewModelList(ocrList).ToArray();
            oVModel.oVModelList = oVModelList;
            return View(oVModel);
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
            ViewBag.caseName = db.Case.Select(x=>x.caseName).ToList();
            //ViewBag.caseName = db.Case.ToList();
            return View();
        }

        // POST: Records/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //public ActionResult Create(string caseName, string departmentCode, string documentCode, string facilityName, string provider, string memo, string serviceDate, string noteSubjective, string history, string noteObjective, string noteAssessment, string notePlan, string medications, string age, string DOB, string allergies, string vitalSigns, string diagnosis)
        public ActionResult Create(CaseRecordViewModel nCRVModel)
        {
            if (ModelState.IsValid)
            {
                Record record = new Record();
                //Department department = new Department();
                //DocumentType type = new DocumentType();
                //Facility facility = new Facility();
                record.departmentId = db.Department.Select(x => x).Where(y => y.departmentCode == nCRVModel.record.Department.departmentCode).First().departmentId;
                record.typeId = db.DocumentType.Select(x => x).Where(y => y.documentCode == nCRVModel.record.DocumentType.documentCode).First().typeId;
                record.facilityId = db.Facility.Select(x => x).Where(y => y.facilityName == nCRVModel.record.Facility.facilityName).First().facilityId;
                record.recordEntryDate = DateTime.Now;
                record.provider = nCRVModel.record.provider;
                record.memo = nCRVModel.record.memo;
                record.serviceDate = nCRVModel.record.serviceDate;
                record.noteSubjective = nCRVModel.record.noteSubjective;
                record.history = nCRVModel.record.history;
                record.noteObjective = nCRVModel.record.noteObjective;
                record.noteAssessment = nCRVModel.record.noteAssessment;
                record.notePlan = nCRVModel.record.notePlan;
                record.medications = nCRVModel.record.medications;
                record.age = nCRVModel.record.age;
                record.DOB = nCRVModel.record.DOB;
                record.allergies = nCRVModel.record.allergies;
                record.vitalSigns = nCRVModel.record.vitalSigns;
                record.diagnosis = nCRVModel.record.diagnosis;
                db.Record.Add(record);
                db.SaveChanges();
                var caseId = db.Case.Select(x => x).Where(y => y.caseName == nCRVModel.thisCase.caseName).First().caseId; 
                record.internalCaseId = db.InternalCaseNumber.Select(x => x).Where(y=>y.caseId == caseId).First().internalCaseId;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View();
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

        //public ActionResult Search(string searchTerm)
        //{
        //    List<Record> recordResultList = SearchRecords(searchTerm);
        //    return View(recordResultList);
        //}

        //public List<Record> SearchRecords(string searchTerm)
        //{
        //    List<Record> recordResultList = new List<Record>();
        //    Record record = new Record();
        //    recordResultList = record.SearchFileContent(searchTerm);
        //    return recordResultList;
        //}

        public StringBuilder DisplayXMLResults(XmlDocument data)
        {

            StringBuilder sb = new StringBuilder();
            foreach (XmlNode node in data.ChildNodes)
            {
                sb.Append(char.ToUpper(node.Name[0]));
                sb.Append(node.Name.Substring(1));
                sb.Append(' ');
                sb.AppendLine(node.InnerText);
            }
            Console.WriteLine(sb);
            return sb;
        }
    }
}
