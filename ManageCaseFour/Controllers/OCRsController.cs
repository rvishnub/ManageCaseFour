using System;
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

namespace ManageCaseFour.Controllers
{
    [Authorize]
    public class OCRsController : Controller, IEnumerable
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        OCRViewModel oVModel;


        // GET: OCRs
        public ActionResult Index(string sortOrder)
        {
            oVModel = new OCRViewModel();
            List<OCR> ocrList = db.OCR.ToList();
            OCRViewModel[] oVModelList = oVModel.GetOCRViewModelList(ocrList).ToArray();
            //ViewBag.DateSortParm = sortOrder == "provider" ? "name_asc" : "";
            //ViewBag.DateSortParm = sortOrder == "serviceDate" ? "date_desc" : "date_asc";
            //Sort(sortOrder, oVModelList);
            oVModel.oVModelList = oVModelList;
            return View(oVModel);
            //var data = oVModelList;
            //return View(oVModelList);
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
        public ActionResult GetAllFilesText(string allFilenames)
        {
            string pageText = "";
            string[] filenames = allFilenames.Split(',');
            for (int pageCount = 0; pageCount<filenames.Count(); pageCount++)
            {
                string text = doOCR(filenames[pageCount]);
                pageText = pageText + " " + text;
            }
            OCR ocr = new OCR();
            ocr.documentText = pageText;
            ocr.documentId = ParseTextIntoSubjects(pageText);
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
            var dataPath = "C:/Users/Renuka/Documents/Visual Studio 2015/Projects/ManageCaseFour/ManageCaseFour/tessdata";
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

        public string ParseTextIntoSubjects(string pageText)
        {
            pageText = pageText.Replace("/n", " ");
            string[] documentSubjects = SplitNoteIntoAllSections(pageText);
            Record record = new Record();
            string noteDate = documentSubjects[2];
            record.serviceDate = GetConvertedDate(noteDate);
            record.provider = documentSubjects[3];
            record.DOB = documentSubjects[4];
            record.age = documentSubjects[5];
            record.noteSubjective = documentSubjects[7];
            record.history = documentSubjects[8];
            record.medications = documentSubjects[9];
            record.allergies = documentSubjects[10];
            record.noteObjective = documentSubjects[11];
            record.noteAssessment = documentSubjects[12];
            record.diagnosis = documentSubjects[13];
            record.notePlan = documentSubjects[14];
            record.recordEntryDate = DateTime.Now;
            record.documentId = record.provider + Convert.ToString(record.serviceDate);
            db.Record.Add(record);
            db.SaveChanges();
            return record.documentId;
;

        }

        public string SplitNoteToGetDocumentId(string text)
        {
            string[] separatingStrings = { "Advanced Pain Management", "Date", "Provider", "Patient", "DOB" };
            string[] sections;
            sections = text.Split(separatingStrings, StringSplitOptions.None);
            string docId = sections[1] + sections[2];
            return docId;
        }


        public string[] SplitNoteIntoAllSections(string text)
        {
            string[] separatingStrings = { "Advanced Pain Management", "Date", "Provider", "Patient", "DOB", "Age", "Sex", "Chief Complaint",
                "History of Present Illness", "Medication", "Allergies", "Vital Signs", "Assessment", "Diagnosis", "Plan" };
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
        public ActionResult Sort(string sortOrder, List<OCRViewModel> oVModelList)

        {
            switch (sortOrder)
            {
                case "date_asc":
                    oVModelList = oVModelList.OrderBy(s => s.serviceDate).ToList();
                    break;
                case "date_desc":
                    oVModelList = oVModelList.OrderByDescending(s => s.serviceDate).ToList();
                    break;
                case "name_asc":
                    oVModelList = oVModelList.OrderBy(s => s.provider).ToList();
                    break;
                default:
                    oVModelList = oVModelList.OrderBy(s => s.serviceDate).ToList();
                    break;
            }
            return View(oVModelList);
        }

        public IEnumerator GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }
}


