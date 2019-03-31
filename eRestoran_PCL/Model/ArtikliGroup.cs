using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eRestoran_PCL.Model
{
    public class ArtikliGroup : List<StavkeMenija>
    {
        public string Heading { get; set; }
        public List<StavkeMenija> Stavke => this;
    }
}
