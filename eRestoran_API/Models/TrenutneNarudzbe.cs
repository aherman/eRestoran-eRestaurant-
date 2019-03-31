using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web;

namespace eRestoran_API.Models
{
    public class TrenutneNarudzbe
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