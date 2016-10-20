using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ManageCaseFour.Models
{
    public class OCR
    {
        [Key]
        public int id { get; set; }
        public string documentId { get; set;}
        public string documentFilename { get; set; }
        public string documentText { get; set; }
        public string documentSections { get; set; }
    }
}