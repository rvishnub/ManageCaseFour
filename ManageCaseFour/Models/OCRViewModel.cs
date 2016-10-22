using System.Collections.Generic;
using ManageCaseFour.Models;
using System.Linq;
using System;
using System.Text;
using System.IO;

namespace ManageCaseFour.Models
{
    public class OCRViewModel
    {
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

        //public string ReadPdfFile(string fileName)
        //{
        //    StringBuilder text = new StringBuilder();

        //    if (File.Exists(fileName))
        //    {
        //        PdfReader pdfReader = new PdfReader(fileName);

        //        for (int page = 1; page <= pdfReader.NumberOfPages; page++)
        //        {
        //            ITextExtractionStrategy strategy = new SimpleTextExtractionStrategy();
        //            string currentText = PdfTextExtractor.GetTextFromPage(pdfReader, page, strategy);

        //            currentText = Encoding.UTF8.GetString(ASCIIEncoding.Convert(Encoding.Default, Encoding.UTF8, Encoding.Default.GetBytes(currentText)));
        //            text.Append(currentText);
        //        }
        //        pdfReader.Close();
        //    }
        //    return text.ToString();
        //}
    }
}
