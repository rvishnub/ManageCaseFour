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

        [ForeignKey("InternalCaseNumber")]
        public int internalCaseId { get; set; }//business associate
        public InternalCaseNumber InternalCaseNumber { get; set; }

        [ForeignKey("Department")]
        public int departmentId { get; set; }//ER, RAD, etc
        public Department Department { get; set; }

        [ForeignKey("DocumentType")]
        public int typeId { get; set; }//note, report, letter
        public DocumentType DocumentType { get; set; }

        [ForeignKey("Facility")]
        public int facilityId { get; set; }//maybe a dropdown
        public Facility Facility { get; set; }


        public string documentId { get; set; } //provider plus date
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
        public string providerFirstName { get; set; }
        public string providerLastName { get; set; }
        public string fileContent { get; set; }
        public string diagnosis { get; set; }

        public List<Record> RecordList = new List<Record>();









        ApplicationDbContext db = new ApplicationDbContext();

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