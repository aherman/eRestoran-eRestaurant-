using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eRestoran_API.Models
{
    public class NarudzbeStavkePost
    {
        public int NarudzbaStavkaID { get; set; }
        public int NarudzbaID { get; set; }
        public int StavkaMenijaID { get; set; }
        public int Kolicina { get; set; }
        public string Napomena { get; set; }

        public StavkeMenijaPut Artikal { get; set; }

        public string UkupnaCijena
        {
            get { return $"{Kolicina * Artikal.Cijena}"; }
        }
    }
}