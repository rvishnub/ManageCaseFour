using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ManageCaseFour.Models
{
    public class NewCaseViewModel
    {
        public InternalCaseNumber intCaseNumber = new InternalCaseNumber();
        public Case newCase = new Case();
        public Principal principal = new Principal();
        //public User staff = new User();
        //public PrincipalCaseJunction prinCaseJunc = new PrincipalCaseJunction();
        //public UserCaseJunction userCaseJunc = new UserCaseJunction();
        public List<ApplicationUser> CaseStaffMembers = new List<ApplicationUser>();
        public List<Case> CaseLoad = new List<Case>();
        public List<Principal> CaseTeam = new List<Principal>();

    }
}