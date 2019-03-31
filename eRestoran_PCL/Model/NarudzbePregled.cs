using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eRestoran_PCL.Model
{
    public class NarudzbePregled : List<NarudzbeStavkePregled>
    {
        public string Heading { get; set; }
        public NarudzbePregled(List<NarudzbeStavkePregled> list, string heading)
        {
            AddRange(list);
            Heading = heading;
        }
    }
}
