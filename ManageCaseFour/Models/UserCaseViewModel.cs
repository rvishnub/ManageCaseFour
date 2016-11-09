using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ManageCaseFour.Models
{
    public class UserCaseViewModel
    {
        public ApplicationUser thisUser { get; set; }
        public Case thisCase { get; set; }

        public UserCaseViewModel[] usersCases = new UserCaseViewModel[] { };

    }
}