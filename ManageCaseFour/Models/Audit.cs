using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace ManageCaseFour.Models
{
    public class Audit
    {
        // Audit Properties
        public Guid AuditID { get; set; }
        public string UserName { get; set; }
        public string IPAddress { get; set; }
        public string AreaAccessed { get; set; }
        public DateTime TimeAccessed { get; set; }

        // Default Constructor
        public Audit() { }

        public class AuditingContext : DbContext
        {
            public DbSet<Audit> AuditRecords { get; set; }
        }

        public class PostingModel
        {
            public string PropertyA { get; set; }
            public string PropertyB { get; set; }

            public PostingModel()
            {

            }
        }

    }
}