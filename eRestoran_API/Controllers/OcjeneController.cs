using eRestoran_API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace eRestoran_API.Controllers
{
    public class OcjeneController : ApiController
    {
        private eRestoranEntities dm = new eRestoranEntities();

        OcjeneController()
        {
            dm.Configuration.LazyLoadingEnabled = false;
        }

        [HttpGet]
        [Route("api/Ocjene")]
        public List<esp_OcjenePrikaz_Result> GetOcjene()
        {
            return dm.esp_OcjenePrikaz().ToList();
        }

        [HttpGet]
        [Route("api/Ocjene/GetById/{id}")]
        public esp_OcjeneStavkaPrikaz_Result GetOcjeneId(int id)
        {
            return dm.esp_OcjeneStavkaPrikaz(id).SingleOrDefault();
        }

        [HttpGet]
        [Route("api/Ocjene/GetOcjeneByStavka/{id}")]
        public List<esp_OcjeneByStavka_Result> GetOcjeneByStavka(int id)
        {
            return dm.esp_OcjeneByStavka(id).ToList();
        }

        [HttpPost]
        [Route("api/Ocjene")]
        public IHttpActionResult PostOcjene(OcjeneVM obj)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            dm.Ocjene.Add(new Ocjene
            {
                StavkaMenijaID = obj.StavkaMenijaID,
                KlijentID = obj.KlijentID,
                Ocjena = obj.Ocjena
            });
            dm.SaveChanges();

            return Ok(obj);
        }

        [HttpGet]
        public Ocjene Rating(int klijentId, int stavkaId)
        {
            Ocjene ocj = dm.Ocjene.Where(x => x.KlijentID == klijentId && x.StavkaMenijaID == stavkaId).SingleOrDefault();
            return ocj;
        }

        [HttpPut]
        [Route("api/Ocjene")]
        public IHttpActionResult PutOcjene(OcjeneVM obj)
        {
            if (obj != null)
            {
                Ocjene ocjena = dm.Ocjene.Where(x => x.KlijentID == obj.KlijentID && x.StavkaMenijaID == obj.StavkaMenijaID).SingleOrDefault();
                ocjena.Ocjena = obj.Ocjena;
                dm.SaveChanges();
                return Ok();
            }
            return NotFound();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                dm.Dispose();

            base.Dispose(disposing);
        }
    }
}
