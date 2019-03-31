using eRestoran_PCL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eRestoran_PCL.Model
{
    public class DetaljiDostavePregled
    {
        public int dostavaId { get; set; }
        public List<TrenutneNarudzbeStavke> stavke { get; set; }
        public string adresaKlijenta { get; set; }
        public string nacinPlacanja { get; set; }
        public KreditneKarticePrikaz kreditnaKartica { get; set; }
    }
}
