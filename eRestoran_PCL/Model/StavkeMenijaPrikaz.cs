using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eRestoran_PCL.Model
{
    public class StavkeMenijaPrikaz
    {
        public int StavkaMenijaID { get; set; }
        public string Naziv { get; set; }
        public decimal Cijena { get; set; }
        public string Kategorija { get; set; }
        public byte[] Slika { get; set; }
    }
}
