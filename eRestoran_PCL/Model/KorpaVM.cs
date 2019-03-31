using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eRestoran_PCL.Model
{
    public class KorpaVM
    {
        public List<KorpaStavke> stavke { get; set; }
        public KorpaVM()
        {
            stavke = new List<KorpaStavke>();
        }
    }

    public class KorpaStavke
    {
        public int stavkaId { get; set; }
        public string naziv { get; set; }
        public int kolicina { get; set; }
        public decimal cijena { get; set; }
        public decimal ukupnaCijena
        {
            get
            {
                return cijena * kolicina;
            }
        }
    }
}
