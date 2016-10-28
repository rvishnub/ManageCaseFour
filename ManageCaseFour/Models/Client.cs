using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ManageCaseFour.Models
{
    public class Client
    {
        [Key]
        public int clientId { get; set; }
        public string clientName { get; set; }

        [ForeignKey("Principal")]
        public int principalId { get; set; }
        public Principal Principal { get; set; }
    }
}