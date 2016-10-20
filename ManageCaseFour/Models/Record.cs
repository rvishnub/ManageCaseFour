using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Web;
using System.IO;

namespace ManageCaseFour.Models
{
    public class Record
    {

        [Key]
        public int recordId { get; set; }

        public int internalCaseId { get; set; }//business associate
        public string InternalCaseNumber { get; set; }

        public int sourceId { get; set; }//plaintiff, defendant, insurance company, patient, provider
        public string DocumentSource { get; set; }

        public int departmentId { get; set; }//ER, RAD, etc
        public string Department { get; set; }

        public string documentId { get; set; } //provider plus date
        public string DocumentType { get; set; }

        public int facilityId { get; set; }//maybe a dropdown
        public string Facility { get; set; }
        public string recordReferenceNumber { get; set; }//autofill
        public string pageNumber { get; set; }
        public DateTime recordEntryDate { get; set; }
        public string provider { get; set; }
        public string memo { get; set; }
        public string noteDate { get; set; }
        public DateTime serviceDate { get; set; }//date of appt
        public string noteSubjective { get; set; }
        public string history { get; set; }
        public string noteObjective { get; set; }
        public string noteAssessment { get; set; }
        public string notePlan { get; set; }
        public string medications { get; set; }
        public string age { get; set; }
        public string DOB { get; set; }
        public string allergies { get; set; }
        public string vitalSigns { get; set; }

        public string diagnosis { get; set; }

        public string fileContent;

        ApplicationDbContext db = new ApplicationDbContext();
        OCR ocr;

        public string GetFileContent(Record record)
        {
            //fileContent = record.documentId + "; " + record.noteSubjective + "; " + record.noteObjective + "; " + record.noteAssessment + "; " + record.diagnosis + "; " + record.notePlan;
            string documentId = record.documentId;
            OCR ocr = db.OCR.Where(x => x.documentId == documentId).FirstOrDefault();
            record.fileContent = ocr.documentText;
            return record.fileContent;
        }

        public List<Record> SearchFileContent(string searchTerm)
        {
            List<Record> recordResultList = new List<Record>();
            List<Record> recordList = db.Record.ToList();
            for (int i = 0; i< recordList.Count(); i++)
            {
                fileContent = GetFileContent(recordList[i]);
                if (fileContent.ToLower().Contains(searchTerm.ToLower()))
                {
                    recordResultList.Add(recordList[i]);
                }

            }
            return recordResultList;
        }

    }
}