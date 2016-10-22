using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ManageCaseFour.Models
{
    public class User
    {
        [Key]
        public int userId { get; set; }
        public int userName { get; set; }
        public string userLastName { get; set; }
        public string userFirstName { get; set; }
        public string userLogin { get; set; }
        public string userPassword { get; set; }
        public string userEmail { get; set; }
        public string userPIN { get; set; }
        public string userPosition { get; set; }//doctor, nurse, attorney, etc
        public string userSecurityQuestion { get; set; }
        public string userSecurityAnswer { get; set; }
    }
}