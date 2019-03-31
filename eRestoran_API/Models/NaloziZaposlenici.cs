using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace eRestoran_API.Models
{
    public class NaloziZaposlenici
    {
        public int zaposlenikID { get; set; }
        public string imePrezime { get; set; }
        public string username { get; set; }
        public string email { get; set; }
        public string uloge { get; set; }
        
    }
}