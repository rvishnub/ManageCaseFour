using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ManageCaseFour.Models
{
    public class Case
    {
        [Key]
        public int caseId { get; set; }
        public string caseName { get; set; }
        public string caseNumber { get; set; }
        public string county { get; set; }

        public Case[] usersCases;

        public ICollection<ApplicationUser> Users { get; set; }


    }
}