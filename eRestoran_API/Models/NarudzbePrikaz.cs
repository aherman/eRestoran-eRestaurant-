using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eRestoran_API.Models
{
    public class NarudzbePrikaz
    {
        public int narudzbaID { get; set; }
        public string imePrezime { get; set; }
        public string telefon { get; set; }
        public string email { get; set; }
        public string datumVrijeme { get; set; }
        public string lokacija { get; set; }
        public string ukupnaCijena { get; set; }
    }
}