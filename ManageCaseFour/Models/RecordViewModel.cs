using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ManageCaseFour.Models
{
    public class RecordViewModel
    {

        public Record record { get; set; }
        public Case thisCase { get; set; }
        public InternalCaseNumber intCaseNumber {get; set;}
        public OCR ocr { get; set; }
        public Department department { get; set; }
        public Facility facility { get; set; }
        public ApplicationUser thisUser { get; set; }

        //public List<RecordViewModel> rCVModelList = new List<RecordViewModel>();
        public RecordViewModel[] rCVModelArray = new RecordViewModel[] { };
    }
}