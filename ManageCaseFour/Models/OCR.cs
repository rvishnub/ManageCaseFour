using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ManageCaseFour.Models
{
    public class OCR
    {
        [Key]
        public int ocrId { get; set; }

        [ForeignKey("Record")]
        public int recordId { get; set; }
        public Record Record { get; set; }

        public string documentFilename { get; set; }
        public string documentText { get; set; }
        public string documentSections { get; set; }
    }
}