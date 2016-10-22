using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ManageCaseFour.Models
{
    public class Facility
    {
        [Key]
        public int facilityId { get; set; }
        public string facilityName { get; set; }

    }
}