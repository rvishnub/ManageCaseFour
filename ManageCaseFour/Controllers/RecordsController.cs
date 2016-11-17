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
using System.Xml;
using Newtonsoft.Json;

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
            RecordViewModel rVModel = new RecordViewModel();
            List<RecordViewModel> rVModelList = new List<RecordViewModel>();
            ApplicationUser myUser = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().
                FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
            ApplicationUser thisUser = myUser;
            List<int> CaseIdList = db.UserCaseJunction.Select(x => x).Where(y => y.Id == thisUser.Id).ToList().Select(w => w.caseId).ToList();
            List<int> InternalCaseIdList = db.InternalCaseNumber.Where(x => CaseIdList.Any(y => y == x.caseId)).ToList().Select(w => w.internalCaseId).ToList();
            List<Record> RecordList = db.Record.Where(y => InternalCaseIdList.Any(z => z == y.internalCaseId)).ToList();
            for (int i = 0; i < RecordList.Count(); i++)
            {
                Record thisRecord = RecordList[i];
                RecordViewModel thisItem = new Models.RecordViewModel();
                //InternalCaseNumber intCaseNumber = new Models.InternalCaseNumber();
                //rCVModel.intCaseNumber.internalCaseNumber = db.InternalCaseNumber.Select(x=>x).Where(y=>y.internalCaseId == thisRecord.internalCaseId).First().internalCaseNumber;
                var caseId = db.InternalCaseNumber.Select(x => x).Where(y => y.internalCaseId == thisRecord.internalCaseId).First().caseId;
                //thisItem.thisCase.caseName = db.Case.Select(x => x).Where(y => y.caseId == caseId).First().caseName;
                thisItem.record = db.Record.Select(x => x).Where(y => y.recordId == thisRecord.recordId).First();
                thisItem.record.recordId = thisRecord.recordId;
                thisItem.department = db.Department.Select(x => x).Where(y => y.departmentId == thisRecord.departmentId).First();
                thisItem.record.serviceDate = thisRecord.serviceDate;
                thisItem.record.provider = thisRecord.provider;
                thisItem.facility = db.Facility.Select(x => x).Where(y => y.facilityId == thisRecord.facilityId).First();
                thisItem.thisCase = db.Case.Select(x => x).Where(y => y.caseId == caseId).First();
                rVModelList.Add(thisItem);
            }
            rVModel.rCVModelArray = rVModelList.ToArray();
            rVModel.rCVModelArray = rVModelList.ToArray();
            return View(rVModel);
            
        }

        // GET: Records/Details/5
        public ActionResult Details(int? recordID)
        {
            if (recordID == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Record record = db.Record.Find(recordID);
            RecordViewModel rVModel = new RecordViewModel();
            rVModel.record = record;
            rVModel.intCaseNumber = db.InternalCaseNumber.Select(x=>x).Where(y=>y.internalCaseId == record.internalCaseId).First();
            rVModel.thisCase = db.Case.Select(x=>x).Where(y=>y.caseId == rVModel.intCaseNumber.caseId).First();
            rVModel.department = db.Department.Select(x=>x).Where(y=>y.departmentId == record.departmentId).First();
            rVModel.facility = db.Facility.Select(x=>x).Where(y=>y.facilityId == record.facilityId).First();

            if (record == null)
            {
                return HttpNotFound();
            }
            return View(rVModel);
        }

        // GET: Records/Create

        [Audit]
        public ActionResult Create()
        {
            ApplicationUser myUser = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().
                FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
            ApplicationUser thisUser = myUser;
            List<int> CaseIdList = db.UserCaseJunction.Select(x => x).Where(y => y.Id == thisUser.Id).ToList().Select(w => w.caseId).ToList();
            List<string> CaseNameList = db.Case.Where(x => CaseIdList.Any(y => y == x.caseId)).ToList().Select(w => w.caseName).ToList();

            ViewBag.caseName = CaseNameList;
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
                Department department = new Department();
                Facility facility = new Facility();
                record.departmentId = db.Department.Select(x => x).Where(y => y.departmentCode == nCRVModel.department.departmentCode).First().departmentId;
                record.facilityId = db.Facility.Select(x => x).Where(y => y.facilityName == nCRVModel.facility.facilityName).First().facilityId;
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
                return RedirectToAction("Index","Records");
            }

            return View();
        }

        // GET: Records/Edit/5
        [Audit]
        public ActionResult Edit(int? recordID)
        {

            if (recordID == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            int id = Convert.ToInt32(recordID);
            RecordViewModel rVModel = new RecordViewModel();
            Record record = db.Record.Find(id);
            InternalCaseNumber intCaseNumber = db.InternalCaseNumber.Find(record.internalCaseId);
            Case thisCase = db.Case.Find(intCaseNumber.caseId);
            Department department = db.Department.Find(record.departmentId);
            Facility facility = db.Facility.Find(record.facilityId);
            rVModel.intCaseNumber = intCaseNumber;
            rVModel.record = record;
            rVModel.thisCase = thisCase;
            rVModel.department = department;
            rVModel.facility = facility;

            if (record == null)
            {
                return HttpNotFound();
            }
            return View(rVModel);
        }

        // POST: Records/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditRecord([Bind(Include = "recordId,Record, internalCaseId,InternalCaseNumber,Case, sourceId,DocumentSource,departmentId,Department,documentId,DocumentType,facilityId,Facility,recordReferenceNumber,pageNumber,recordEntryDate,provider,memo,serviceDate,noteSubjective,history,noteObjective,noteAssessment,notePlan,medications,age,DOB,allergies,vitalSigns,diagnosis,fileContent")] RecordViewModel rVModel)
        {
            if (ModelState.IsValid)
            {
                rVModel.record.departmentId = db.Department.Select(x => x).Where(y => y.departmentCode == rVModel.department.departmentCode).First().departmentId;
                rVModel.record.facilityId = db.Facility.Select(x => x).Where(y => y.facilityName == rVModel.facility.facilityName).First().facilityId;
                db.Entry(rVModel.record).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(rVModel);
        }

        public ActionResult Add()
        {
            ViewBag.departmentCode = db.Department.Select(x => x.departmentCode).ToList();
            ViewBag.facilityName = db.Facility.Select(y => y.facilityName).ToList();
            return View();
        }

        // GET: Records/Delete/5
        [Audit]
        public ActionResult Delete(int? recordID)
        {
            if (recordID == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            int id = Convert.ToInt32(recordID);
            RecordViewModel rVModel = new RecordViewModel();
            Record record = db.Record.Find(id);
            InternalCaseNumber intCaseNumber = db.InternalCaseNumber.Find(record.internalCaseId);
            Case thisCase = db.Case.Find(intCaseNumber.caseId);
            Department department = db.Department.Find(record.departmentId);
            Facility facility = db.Facility.Find(record.facilityId);
            rVModel.intCaseNumber = intCaseNumber;
            rVModel.record = record;
            rVModel.thisCase = thisCase;
            rVModel.department = department;
            rVModel.facility = facility;

            if (record == null)
            {
                return HttpNotFound();
            }
            return View(rVModel);
        }
   
        // POST: Records/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int recordID)
        {
            Record record = db.Record.Find(recordID);
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


        public ActionResult AddDeptFacility(string departmentCode, string facilityName)
        {
            RecordViewModel rCVModel = new RecordViewModel();
            Department department = new Department();
            Facility facility = new Facility();
            List<string> DeptCodes = db.Department.Select(x => x.departmentCode).ToList();
            List<string> FacNames = db.Facility.Select(x => x.facilityName).ToList();

            if (ModelState.IsValid)
            {
                if (departmentCode != null && departmentCode != "")
                {
                    if (DeptCodes.Contains(departmentCode))
                    {
                        return RedirectToAction("Error", "Records");
                    }
                    department.departmentCode = departmentCode;
                    db.Department.Add(department);
                }

                if (facilityName != null && facilityName != "")
                {
                    if (FacNames.Contains(facilityName))
                    {
                        return RedirectToAction("Error", "Records");

                    }
                    facility.facilityName = facilityName;
                    db.Facility.Add(facility);
                }
                db.SaveChanges();
            }
            return RedirectToAction("Index", "Case");
        }

        public string DisplayXMLResults(XmlDocument xml)
        {
            //XmlDocument doc = new XmlDocument();
            //doc.LoadXml(data);
            string json = JsonConvert.SerializeObject(xml);
            return json;
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
