using System;
using System.Collections;
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
        public ApplicationUser staff = new ApplicationUser();
        public PrincipalCaseJunction prinCaseJunc = new PrincipalCaseJunction();
        public UserCaseJunction userCaseJunc = new UserCaseJunction();
        public List<ApplicationUser> CaseStaffMembers = new List<ApplicationUser>();
        public List<Principal> CaseTeam = new List<Principal>();
        public List<NewCaseViewModel> CaseLoad = new List<NewCaseViewModel>();
        public List<NewCaseViewModel> oVModelList = new List<NewCaseViewModel>();
        public NewCaseViewModel[] nCVModelList = new NewCaseViewModel[] { };

        public NewCaseViewModel()
        {

        }
    }
}