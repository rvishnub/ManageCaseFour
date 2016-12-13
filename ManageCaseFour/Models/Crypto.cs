using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ManageCaseFour.Models
{
    public class Crypto
    {

        [Key]
        public int arrayId { get; set; }
        public string filename { get; set; }
        public byte[] encryptedOriginal { get; set; }
        public byte[] key { get; set; }
        public byte[] IV { get; set; }

    }
}