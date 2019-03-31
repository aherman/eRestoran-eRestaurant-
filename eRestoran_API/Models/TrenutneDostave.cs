using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eRestoran_API.Models
{
    public class TrenutneDostave
    {
        public int dostavaId { get; set; }
        public string imePrezime { get; set; }
        public List<TrenutneNarudzbeStavke> stavke { get; set; }
    }
}
