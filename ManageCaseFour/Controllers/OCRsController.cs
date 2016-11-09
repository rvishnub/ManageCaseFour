using System;
using System.IO;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ManageCaseFour.Models;
using Tesseract;
using System.Web.Script.Serialization;
using System.Globalization;
using static ManageCaseFour.Controllers.AuditsController;
using System.Collections;
using System.Text;
using System.Xml;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity;

namespace ManageCaseFour.Controllers
{
    [Authorize(Roles = "Employee, Manager")]
    public class OCRsController : Controller, IEnumerable
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        OCRViewModel oVModel;


        // GET: OCRs
        public ActionResult Index(string sortOrder)
        {
            oVModel = new OCRViewModel();
            List<OCR> ocrList = db.OCR.ToList();

            ApplicationUser myUser = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().
                FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
            ApplicationUser thisUser = myUser;
            //List<int> CaseIdList = db.UserCaseJunction.Select(x => x).Where(y => y.Id == thisUser.Id).ToList().Select(w => w.caseId).ToList();
            //List<Case> UserCaseList = db.Case.Where(x => CaseIdList.Any(y => y == x.caseId)).ToList();
            //List<string> UserCaseNameList = UserCaseList.Select(x => x.caseName).ToList();
            //oVModel.CaseListArray = UserCaseList.ToArray();
            //OCRViewModel[] oVModelList = oVModel.GetOCRViewModelList(ocrList).Where(x => UserCaseNameList.Any(z => z == x.caseName)).ToArray();

            //oVModel.oVModelList = oVModelList;
            return View(oVModel);
        }

        // GET: OCRs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OCR oCR = db.OCR.Find(id);
            if (oCR == null)
            {
                return HttpNotFound();
            }
            return View(oCR);
        }

        // GET: OCRs/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: OCRs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,documentId,documentFilename,documentText")] OCR oCR)
        {
            if (ModelState.IsValid)
            {
                db.OCR.Add(oCR);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(oCR);
        }

        // GET: OCRs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OCR oCR = db.OCR.Find(id);
            if (oCR == null)
            {
                return HttpNotFound();
            }
            return View(oCR);
        }

        // POST: OCRs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Audit]
        public ActionResult Edit([Bind(Include = "id,documentId,documentFilename,documentText")] OCR oCR)
        {
            if (ModelState.IsValid)
            {
                db.Entry(oCR).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(oCR);
        }

        // GET: OCRs/Delete/5
        [Audit]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OCR oCR = db.OCR.Find(id);
            if (oCR == null)
            {
                return HttpNotFound();
            }
            return View(oCR);
        }

        // POST: OCRs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Audit]
        public ActionResult DeleteConfirmed(int id)
        {
            OCR oCR = db.OCR.Find(id);
            db.OCR.Remove(oCR);
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

        [Audit]
        public ActionResult GetAllFilesText(string allFilenames, string caseID)
        {
            string pageText = "";
            string[] filenames = allFilenames.Split(',');
            for (int pageCount = 0; pageCount < filenames.Count(); pageCount++)
            {
                string text = doOCR(filenames[pageCount]);
                pageText = pageText + " " + text;
            }
            int caseId = Convert.ToInt32(caseID);
            string caseName = db.Case.Select(x => x).Where(y => y.caseId == caseId).First().caseName;
            OCR ocr = new OCR();
            ocr.documentText = pageText;
            ocr.recordId = ParseTextIntoSubjects(pageText, caseName);
            ocr.documentFilename = filenames[0];
            ocr.documentText = pageText;
            db.OCR.Add(ocr);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        //THIS CODE FROM www.dotnetfunda.com/articles/show/3220/extract-text-from-image-using-tesseract-in-csharp.  
        //DATA FILE FROM bhttps://github.com/bytedeco/sample-projects.  
        //Tesseract package from NuGet.
        public string doOCR(string filename)
        {
            var testImagePath = filename;
            var dataPath = "C:/Users/Renuka/Documents/GitHub/ManageCaseFour/ManageCaseFour/tessdata";
            //var dataPath = "./tessdata";
            using (var tEngine = new TesseractEngine(dataPath, "eng", EngineMode.Default)) //creating the tesseract OCR engine with English as the language
            {
                using (var img = Pix.LoadFromFile(testImagePath)) // Load of the image file from the Pix object which is a wrapper for Leptonica PIX structure
                {
                    using (var page = tEngine.Process(img)) //process the specified image
                    {

                        var text = page.GetText(); //Gets the image's content as plain text.
                                                    //OCR ocr = new OCR();
                                                    //ocr.documentId = ParseTextIntoSubjects(pageText);
                                                    //ocr.documentFilename = filename;
                                                    //db.OCR.Add(ocr);
                                                    //db.SaveChanges();
                        return text;
                    }

                }
            }

        }

        public int ParseTextIntoSubjects(string pageText, string caseName)
        {
            Case thisCase = new Models.Case();
            pageText = pageText.Replace("/n", " ");
            string[] documentSubjects = SplitNoteIntoAllSections(pageText);
            Record record = new Record();
            string noteDate = documentSubjects[2];
            record.serviceDate = GetConvertedDate(noteDate);
            record.provider = documentSubjects[3];
            record.noteSubjective = documentSubjects[6]+" "+documentSubjects[7]+" "+documentSubjects[8];
            record.history = documentSubjects[8];
            record.medications = documentSubjects[9];
            record.noteObjective = documentSubjects[11] +" " + documentSubjects[12];
            record.noteAssessment = documentSubjects[13];
            record.diagnosis = documentSubjects[14];
            record.notePlan = documentSubjects[14];
            record.recordEntryDate = DateTime.Now;
            thisCase.caseId = db.Case.Select(x => x).Where(y => y.caseName == caseName).First().caseId;
            int internalCaseId = db.InternalCaseNumber.Select(x => x).Where(y => y.caseId == thisCase.caseId).First().internalCaseId;
            record.internalCaseId = internalCaseId;
            thisCase.caseName = caseName;
            record.departmentId = 1;
            record.facilityId = 1;
            db.Record.Add(record);
            db.SaveChanges();
            return record.recordId; //fix this, was ocrId, has to be something else now

        }

        public string[] SplitNoteIntoAllSections(string text)
        {
            string[] separatingStrings = { "Advanced Pain Management", "Date", "Provider", "DOB","Age","Sex","Chief Complaint",
            "History of Present Illness", "Medication", "Allergies", "Vital Signs", "Physical Exam","Assessment", "Diagnosis", "Plan" };
            string[] documentSections;
            documentSections = text.Split(separatingStrings, StringSplitOptions.None);
            return documentSections;
        }

        public DateTime GetConvertedDate(string date)
        {
            DateTime formattedDate;
            try
            {
                date = date.Remove(0, 1);
                formattedDate = Convert.ToDateTime(date);

            }
            catch
            {
                formattedDate = DateTime.Now;
            }
            return formattedDate;
        }

        //Sorting method from www.asp.net/mvc/overview/getting-started/getting-started-with-ef-using-mvc/sorting-filtering-and-paging-with-the-entity-framework-in-an-asp-net-mvc-application

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


        public IEnumerator GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }
}


