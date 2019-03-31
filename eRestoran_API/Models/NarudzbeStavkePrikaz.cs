using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eRestoran_API.Models
{
    public class NarudzbeStavkePrikaz
    {
        public int narudzbaStavkaID { get; set; }
        public string naziv { get; set; }
        public string cijena { get; set; }
        public int kolicina { get; set; }
        public string napomena { get; set; }
        public string ukupnaCijena { get; set; }
    }
}