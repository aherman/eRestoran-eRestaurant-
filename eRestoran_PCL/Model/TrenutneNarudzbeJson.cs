using System;
using System.Collections.Generic;
using System.Linq;

namespace eRestoran_PCL.Model
{
    public class TrenutneNarudzbeJson
    {
        public int narudzbaID { get; set; }
        public DateTime datum { get; set; }
        public List<TrenutneNarudzbeStavke> stavke { get; set; }
        public bool aktivna { get; set; }
    }

    public class TrenutneNarudzbeStavke
    {
        public int narudzbaStavkaID { get; set; }
        public int kolicina { get; set; }
        public string naziv { get; set; }
        public decimal cijena { get; set; }
    }
}