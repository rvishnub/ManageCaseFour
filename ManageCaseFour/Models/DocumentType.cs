using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ManageCaseFour.Models
{
    public class DocumentType
    {
        [Key]
        public int typeId { get; set; }
        public string documentCode { get; set; }
        public string documentName { get; set; }

    }
}