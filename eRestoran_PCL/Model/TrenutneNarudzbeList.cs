using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;

namespace eRestoran_PCL.Model
{
    public class TrenutneNarudzbeList : ObservableCollection<TrenutneNarudzbeListStavke>
    {
        public int narudzbaID { get; set; }
        public DateTime datum { get; set; }
        public string color { get; set; }
        public bool aktivna { get; set; }
    }

    public class TrenutneNarudzbeListStavke
    {
        public int narudzbaStavkaID { get; set; }
        public int kolicina { get; set; }
        public string naziv { get; set; }
        public TrenutneNarudzbeListStavke()
        {
        }
    }
}