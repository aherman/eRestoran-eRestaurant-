using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eRestoran_PCL.Model
{
    public class NarudzbeStavke
    {
        public int NarudzbaStavkaID { get; set; }
        public int NarudzbaID { get; set; }
        public int StavkaMenijaID { get; set; }
        public int Kolicina { get; set; }
        public string Napomena { get; set; }

        public StavkeMenija Artikal { get; set; }

        public string UkupnaCijena
        {
            get { return $"{Kolicina * Artikal.Cijena}"; }
        }
    }
}
