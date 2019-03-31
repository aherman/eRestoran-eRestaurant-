using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eRestoran_PCL.Model
{
    public class Dostave
    {
        public int DostavaID { get; set; }
        public int NarudzbaID { get; set; }
        public bool IsZavrsena { get; set; }
        public bool Prioritetna { get; set; }
        public DateTime? DatumSlanja { get; set; }
        public DateTime? DatumPreuzimanja { get; set; }
    }
}
