using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ManageCaseFour.Models
{
    public class ClassViewModel
    {
        public Case thisCase { get; set; }
        public Department department { get; set; }
        public Facility facility { get; set; }
        public InternalCaseNumber intCaseNumber { get; set; }
        public OCR ocr { get; set; }
        public PatientProfile profile {get; set;}



    }
}