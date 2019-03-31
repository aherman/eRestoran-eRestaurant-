using eRestoran_API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace eRestoran_API.Controllers
{
    public class IzvjestajiController : ApiController
    {
        private eRestoranEntities dm = new eRestoranEntities();

        [HttpGet]
        [Route("api/Izvjestaji/Top5Prodanih")]
        public List<esp_IzvjestajTop5ProdavanihArtikala_Result> GetTop5ProdaniArtikli()
        {
            return dm.esp_IzvjestajTop5ProdavanihArtikala().ToList();
        }

        [HttpGet]
        [Route("api/Izvjestaji/ProdaniArtikliKlijent/{klijentID}")]
        public List<esp_IzvjestajProdatiArtikliKupac_Result> GetProdaniArtikliByKlijent(int klijentID)
        {
            return dm.esp_IzvjestajProdatiArtikliKupac(klijentID).ToList();
        }

        [HttpGet]
        [Route("api/Izvjestaji/Top5Najskupljih")]
        public List<esp_IzvjestajTop5NajskupljihNarudzbi_Result> GetTop5NajskupljihNarudzbi()
        {
            return dm.esp_IzvjestajTop5NajskupljihNarudzbi().ToList();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                dm.Dispose();

            base.Dispose(disposing);
        }
    }
}
