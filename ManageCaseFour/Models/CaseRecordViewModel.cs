using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ManageCaseFour.Models
{
    public class CaseRecordViewModel
    {
        public Record record { get; set; }
        public Case thisCase { get; set; }
        public OCR ocr { get; set; }
        public Department department { get; set; }
        public DocumentType type { get; set; }
        public Facility facility { get; set; }
        public InternalCaseNumber internalCaseNumber { get; set; }
        

        ApplicationDbContext db = new ApplicationDbContext();

        List<Case> CaseList = new List<Case>();

        public List<Case> GetCaseList()
        {
            CaseList = db.Case.ToList();
            return CaseList;
        }

    }
}