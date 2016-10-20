using System.Collections.Generic;
using ManageCaseFour.Models;
using System.Linq;
using System;

namespace ManageCaseFour.Models
{
    public class OCRViewModel
    {
        private OCR ocr;
        private Record record;

        public string documentId { get; set; }
        public string documentFilename { get; set; }
        public string documentText { get; set; }
        public DateTime serviceDate { get; set; }
        public string provider { get; set; }

        ApplicationDbContext db = new ApplicationDbContext();
        List<OCR> ocrList = new List<OCR>();
        List<OCRViewModel> ovModel = new List<OCRViewModel>();
        public OCRViewModel[] oVModelList = new OCRViewModel[] { };


        public List<OCRViewModel> GetOCRViewModelList(List<OCR> ocrList)
        {
            for (int i = 0; i < ocrList.Count(); i++)
            {
                OCRViewModel modelItem = new OCRViewModel();
                modelItem.documentId = ocrList[i].documentId;
                record = db.Record.Where(t => t.documentId == modelItem.documentId).FirstOrDefault();
                modelItem.serviceDate = record.serviceDate;
                modelItem.provider = record.provider;
                modelItem.documentText = ocrList[i].documentText;
                modelItem.documentFilename = ocrList[i].documentFilename;
                ovModel.Add(modelItem);
            }
            return ovModel;
        }
    }
}
