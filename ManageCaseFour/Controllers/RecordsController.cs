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
        CaseRecordViewModel cRVModel;
        ClassViewModel cVM;

        // GET: Records
        public ActionResult Index()
        {
            ClassViewModel cVM = new ClassViewModel();
            RecordViewModel rCVModel = new RecordViewModel();
            List<RecordViewModel> rCVModelList = new List<RecordViewModel>();
            List<Record> RecordList = db.Record.ToList();
            for (int i = 0; i<RecordList.Count(); i++)
            {
                Record thisRecord = RecordList[i];
                RecordViewModel thisItem = new Models.RecordViewModel();
                //InternalCaseNumber intCaseNumber = new Models.InternalCaseNumber();
                //rCVModel.intCaseNumber.internalCaseNumber = db.InternalCaseNumber.Select(x=>x).Where(y=>y.internalCaseId == thisRecord.internalCaseId).First().internalCaseNumber;
                var caseId = db.InternalCaseNumber.Select(x => x).Where(y=>y.internalCaseId == thisRecord.internalCaseId).First().caseId;
                var caseName = db.Case.Select(x => x).Where(y => y.caseId == caseId).First().caseName;
                thisItem.record = db.Record.Select(x=>x).Where(y=>y.recordId == thisRecord.recordId).First();
                thisItem.record.recordId = thisRecord.recordId;
                thisItem.department = db.Department.Select(x => x).Where(y => y.departmentId == thisRecord.departmentId).First();
                thisItem.record.serviceDate = thisRecord.serviceDate;
                thisItem.record.provider = thisRecord.provider;
                thisItem.facility = db.Facility.Select(x => x).Where(y => y.facilityId == thisRecord.facilityId).First();
                thisItem.thisCase = db.Case.Select(x => x).Where(y => y.caseId == caseId).First();
                rCVModelList.Add(thisItem);
            }
            //rCVModel.rCVModelArray = rCVModelList.ToArray();
            rCVModel.rCVModelArray = rCVModelList.ToArray();
            return View(rCVModel);
            
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
            ViewBag.departmentCode = db.Department.Select(x => x.departmentCode).ToList();
            ViewBag.facilityName = db.Facility.Select(x => x.facilityName).ToList();

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
                //Facility facility = new Facility();
                record.departmentId = db.Department.Select(x => x).Where(y => y.departmentCode == nCRVModel.record.Department.departmentCode).First().departmentId;
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
                var caseId = db.Case.Select(x => x).Where(y => y.caseName == nCRVModel.thisCase.caseName).First().caseId; 
                record.internalCaseId = db.InternalCaseNumber.Select(x => x).Where(y=>y.caseId == caseId).First().internalCaseId;
                db.Record.Add(record);
                db.SaveChanges();
                return RedirectToAction("Index","Home");
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
            Record record = new Record();
            record = db.Record.Select(x => x).Where(y => y.recordId == id).First();
            ClassViewModel cVM = new ClassViewModel();
            cVM.intCaseNumber.caseId = db.InternalCaseNumber.Select(x => x).Where(y => y.internalCaseId == record.internalCaseId).First().caseId;
            cVM.thisCase.caseName = db.Case.Select(x => x).Where(y => y.caseId == cVM.intCaseNumber.caseId).First().caseName;
            cVM.department.departmentCode = db.Department.Select(x => x).Where(y => y.departmentId == record.departmentId).First().departmentCode;
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
            return sb;
        }

        public void AddDepartment(string code)
        {
            Department department = new Models.Department();
            department.departmentCode = code;
            List<string> codeList = db.Department.Select(x => x.departmentCode).ToList();
            try
            {
                if (!codeList.Contains(code))
                {
                    db.Department.Add(department);
                    db.SaveChanges();
                }
            }
            catch
            {
                //alert(error);
            }

        }

        public void AddFacility(string name)
        {
            Facility facility = new Facility();
            facility.facilityName = name;
            List<string> nameList = db.Facility.Select(x => x.facilityName).ToList();
            try
            {
                if (!nameList.Contains(name))
                {
                    db.Facility.Add(facility);
                    db.SaveChanges();
                }
            }
            catch
            {
                //alert(error);
            }
        }

    }
}
