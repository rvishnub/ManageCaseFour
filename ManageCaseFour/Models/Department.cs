using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ManageCaseFour.Models
{
    public class Department
    {
        [Key]
        public int departmentId { get; set; }
        public string departmentCode { get; set; }
        public string departmentName { get; set; }

    }
}