using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eRestoran_API.Models
{
    public class NaloziKlijenti
    {
        public int klijentID { get; set; }
        public string imePrezime { get; set; }
        public string username { get; set; }
        public string email { get; set; }
        public string adresa { get; set; }
    }
}