using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ManageCaseFour.Models
{
    public class UserCaseJunction
    {
        [Key]
        public int userCaseJunctionId { get; set; }

        [ForeignKey("User")]
        public int userId { get; set; }
        public User User { get; set; }

        [ForeignKey("Case")]
        public int caseId { get; set; }
        public Case Case { get; set; }
    }
}