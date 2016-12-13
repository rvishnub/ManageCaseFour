using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ManageCaseFour.Models;
using System.IO;
using System.Security.Cryptography;

namespace ManageCaseFour.Controllers
{
    [Authorize(Roles = "Manager")]
    public class CaseController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        NewCaseViewModel oVModel = new NewCaseViewModel();
        // GET: Case
        public ActionResult Index()
        {
            List<Case> CaseList = db.Case.ToList();
            List<NewCaseViewModel> oVModelList = new List<NewCaseViewModel>();
            List<NewCaseViewModel> CaseLoad = new List<NewCaseViewModel>();
            for (int i = 0; i < CaseList.Count(); i++)
            {
                Case newCase = CaseList[i];
                PrincipalCaseJunction pCJunc = new PrincipalCaseJunction();
                pCJunc = db.PrincipalCaseJunction.Select(x => x).Where(y => y.caseId == newCase.caseId).First();
                Principal principal = new Principal();
                principal.principalCode = db.Principal.Select(x => x).Where(y => y.principalId == pCJunc.principalId).First().principalCode;
                var caseIdList = db.UserCaseJunction.Select(x => x).Where(y => y.caseId == newCase.caseId).ToList();
                for (int j = 0; j < caseIdList.Count(); j++)
                {
                    NewCaseViewModel oVModel = new NewCaseViewModel();
                    var usercaseId = caseIdList[j];
                    ApplicationUser user = db.Users.Select(x => x).Where(y => y.Id == usercaseId.Id).First();
                    oVModel.staff = user;
                    oVModel.principal.principalCode = principal.principalCode;
                    oVModel.newCase = newCase;
                    oVModelList.Add(oVModel);
                }
                oVModel.CaseLoad = oVModelList;
                NewCaseViewModel[] nCVModelList = oVModel.CaseLoad.ToArray();
                oVModel.nCVModelList = nCVModelList;
            }
            return View(oVModel);
        }



        // GET: Case/Details/5
        public ActionResult Details(string someId, string param)
        {
            if (someId == "" || param == "")
            {
                oVModel.CaseLoad = new List<NewCaseViewModel>();
            }
            else if (param == "Employee")
            {
                List<Case> CaseList = db.Case.ToList();
                for (int i = 0; i < CaseList.Count(); i++)
                {
                    List<NewCaseViewModel> oVModelList = new List<NewCaseViewModel>();
                    Case newCase = CaseList[i];
                    var principalId = db.PrincipalCaseJunction.Select(x => x).Where(y => y.caseId == newCase.caseId).First();
                    Principal principal = db.Principal.Select(y => y).Where(x => x.principalId == principalId.principalId).First();
                    //var caseIdList = db.UserCaseJunction.Select(x => x).Where(y => y.caseId == newCase.caseId).ToList();
                    //for (int j = 0; j < caseIdList.Count(); j++)
                    //{
                    //    NewCaseViewModel oVModel = new NewCaseViewModel();
                    //    var usercaseId = caseIdList[j];
                    //    ApplicationUser user = db.Users.Select(x => x).Where(y => y.Id == usercaseId.Id).First();
                    //    oVModel.staff = user;
                    //    oVModel.principal = principal;
                    //    oVModel.newCase = newCase;
                    //    oVModelList.Add(oVModel);
                    //}
                    oVModel.oVModelList = oVModelList;
                    List<NewCaseViewModel> CaseLoad = new List<NewCaseViewModel>();
                    for (int k = 0; k < oVModelList.Count(); k++)
                    {
                        if (oVModelList[k].staff.UserName == someId)
                        {
                            CaseLoad.Add(oVModelList[k]);
                        }
                        oVModel.CaseLoad = CaseLoad;
                    }
                }
            }
            else if (param == "Principal")
            {
                List<NewCaseViewModel> oVModelList = new List<NewCaseViewModel>();
                List<Case> CaseList = db.Case.ToList();
                for (int i = 0; i < CaseList.Count(); i++)
                {
                    var eachCase = CaseList[i];
                    var principalId = db.PrincipalCaseJunction.Select(x => x).Where(y => y.caseId == eachCase.caseId).First();
                    Principal principal = db.Principal.Select(y => y).Where(x => x.principalId == principalId.principalId).First();
                    if (principal.principalCode == someId)
                    {
                        NewCaseViewModel oVModel = new NewCaseViewModel();
                        oVModel.principal = principal;
                        oVModel.newCase = CaseList[i];
                        oVModelList.Add(oVModel);
                    }
                    oVModel.CaseLoad = oVModelList;
                }
            }
            NewCaseViewModel[] nCVModelList = oVModel.CaseLoad.ToArray();
            oVModel.nCVModelList = nCVModelList;
            return View(oVModel);
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
        //public ActionResult Create([Bind(Include = "caseId,caseName,caseNumber, principalCode, UserName")] NewCaseViewModel oVModel)
        public ActionResult Create(string caseName, string county, string caseNumber, string principalCode, string UserName, int internalCaseNumber)
        {
            if (ModelState.IsValid)
            {
                Case newCase = new Case();
                newCase.county = county;
                newCase.caseNumber = caseNumber;
                newCase.caseName = caseName;
                db.Case.Add(newCase);
                db.SaveChanges();
                Principal principal = new Principal();
                principal.principalCode = principalCode;
                db.Principal.Add(principal);
                db.SaveChanges();
                PrincipalCaseJunction pCJunc = new PrincipalCaseJunction();
                pCJunc.caseId = newCase.caseId;
                pCJunc.principalId = db.Principal.Select(x => x).Where(y => y.principalCode == principalCode).First().principalId;
                db.PrincipalCaseJunction.Add(pCJunc);
                UserCaseJunction uCJunc = new UserCaseJunction();
                uCJunc.caseId = newCase.caseId;
                uCJunc.Id = db.Users.Select(x => x).Where(y => y.UserName == UserName).First().Id;
                db.UserCaseJunction.Add(uCJunc);
                db.SaveChanges();
                InternalCaseNumber intCaseNumber = new InternalCaseNumber();
                intCaseNumber.internalCaseNumber = internalCaseNumber;
                intCaseNumber.caseId = newCase.caseId;
                intCaseNumber.caseEntryDate = DateTime.Now;
                db.InternalCaseNumber.Add(intCaseNumber);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View();
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

        public ActionResult EncryptRecords()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EncryptRecords(string filename)
        {
            string byteArrayFilename = Path.ChangeExtension(filename, null) + "encrypted";

            Rijndael myRin = Rijndael.Create();
            byte[] key = myRin.Key;
            byte[] IV = myRin.IV;
            ICryptoTransform encryptor = myRin.CreateEncryptor(key, IV);
            Crypto crypto = new Models.Crypto();
            CryptoViewModel cryptoVM = new CryptoViewModel();
            byte[] original = cryptoVM.ConvertImageToByteArray(filename);
            byte[] encryptedOriginal = cryptoVM.EncryptArrayToBytes(original, encryptor, key, IV);
            bool result = cryptoVM.SaveByteArray(encryptedOriginal, byteArrayFilename);
            crypto.filename = filename;
            crypto.key = key;
            crypto.IV = IV;
            crypto.encryptedOriginal = encryptedOriginal;
            db.Crypto.Add(crypto);
            db.SaveChanges();

            //this here for testing
            ICryptoTransform decryptor = myRin.CreateDecryptor(key, IV);
            byte[] decryptedOriginal = cryptoVM.DecryptBytesToArray(encryptedOriginal, decryptor, key, IV);
            cryptoVM.ConvertByteArrayToImage(decryptedOriginal, Path.ChangeExtension(filename, null) + "decrypted"); 
            //this here for testing

            return View();
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
