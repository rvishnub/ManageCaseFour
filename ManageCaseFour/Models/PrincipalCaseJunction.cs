using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ManageCaseFour.Models
{
    public class PrincipalCaseJunction
    {
        [Key]
        public int principalCaseJunctionId { get; set; }

        [ForeignKey("Principal")]
        public int principalId { get; set; }
        public Principal Principal { get; set; }

        [ForeignKey("Case")]
        public int caseId { get; set; }
        public Case Case { get; set; }
    }
}