using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eRestoran_API.Models
{
    public class NarudzbeUredi
    {
        public NarudzbeUredi()
        {
            NarudzbeStavke = new List<NarudzbeStavke>();
        }

        public int NarudzbaID { get; set; }
        public int KlijentID { get; set; }
        public DateTime Datum { get; set; }
        public bool Aktivna { get; set; }
        public bool IsGotovina { get; set; }

        public bool PrioritetnaDostava { get; set; }
        public string AdresaDostave { get; set; }


        public KreditneKartice kartica { get; set; }

        public List<NarudzbeStavke> NarudzbeStavke { get; set; }
    }
}