using eRestoran_API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eRestoran_API.Controllers
{
    public class NarudzbePost
    {
        public NarudzbePost()
        {
            NarudzbeStavke = new List<NarudzbeStavkePost>();
        }

        public int NarudzbaID { get; set; }
        public int KlijentID { get; set; }
        public DateTime Datum { get; set; }
        public bool Aktivna { get; set; }
        public bool IsGotovina { get; set; }

        public bool PrioritetnaDostava { get; set; }
        public string AdresaDostave { get; set; }

        public KreditneKarticePut kartica { get; set; }

        public List<NarudzbeStavkePost> NarudzbeStavke { get; set; }
    }
}