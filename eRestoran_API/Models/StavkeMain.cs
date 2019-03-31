using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eRestoran_API.Models
{
    public class StavkeMain
    {
        public int StavkaMenijaID { get; set; }
        public int TipStavkeID { get; set; }
        public int? PopustID { get; set; }
        public string Sifra { get; set; }
        public string Naziv { get; set; }
        public decimal Cijena { get; set; }
        public string Opis { get; set; }
        public byte[] SlikaThumb { get; set; }
        public bool Status { get; set; }
    }
}