using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eRestoran_API.Models
{
    public class DetaljiDostave
    {
        public int dostavaId { get; set; }
        public List<TrenutneNarudzbeStavke> stavke { get; set; }
        public string adresaKlijenta { get; set; }
        public string nacinPlacanja { get; set; }
        public KreditneKarticePrikaz kreditnaKartica { get; set; }
    }
}