using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ManageCaseFour.Models
{
    public class PatientProfile
    {
        public int patientprofileId {get; set;}
        public string patientFirstName { get; set; }
        public string patientLastName { get; set; }
        public DateTime dateOfBirth { get; set; }
        public DateTime dateOfAccident { get; set; }
    }
}