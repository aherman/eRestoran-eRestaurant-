using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eRestoran_API.Models
{
    public class ProdaniArtikliPrikaz
    {
        public string Naziv { get; set; }
        public string Sifra { get; set; }
        public decimal Cijena { get; set; }
        public int Kolicina { get; set; }
        public decimal UkupnaCijena { get; set; }
    }
}