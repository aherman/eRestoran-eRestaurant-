using eRestoran_API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using System.Data.Entity;

namespace eRestoran_API.Controllers
{
    public class TipoviStavkeController : ApiController
    {
        private eRestoranEntities dm = new eRestoranEntities();

        TipoviStavkeController()
        {
            dm.Configuration.LazyLoadingEnabled = false;
        }

        [HttpGet]
        [Route("api/TipoviStavke")]
        public List<TipoviStavke> GetTipoviStavke()
        {
            dm.Configuration.LazyLoadingEnabled = false;
            return dm.TipoviStavke.ToList();
        }

        [HttpGet]
        [Route("api/TipoviStavke/GetById/{id}")]
        public TipoviStavke GetTipoviStavkeId(int id)
        {
            return dm.TipoviStavke.Find(id);
        }

        [HttpPost]
        [Route("api/TipoviStavke")]
        public IHttpActionResult PostTipoviStavke(TipoviStavke obj)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            dm.TipoviStavke.Add(obj);
            dm.SaveChanges();

            return Ok(obj);
        }

        [Route("DELETE")]
        [Route("api/TipoviStavke/Obrisi/{id}")]
        public IHttpActionResult DeleteTipoviStavke(int id)
        {
            TipoviStavke ts = dm.TipoviStavke
                .Include(x=>x.StavkeMenija)
                .Where(x=>x.TipStavkeID == id).SingleOrDefault();

            foreach (StavkeMenija sm in ts.StavkeMenija.ToList())
            {
                foreach (var item in dm.Ocjene.Where(x => x.StavkaMenijaID == sm.StavkaMenijaID).ToList())
                {
                    dm.Ocjene.Remove(item);
                }

                foreach (var n in dm.Narudzbe.ToList())
                {
                    List<NarudzbeStavke> narudzbeStavke = dm.NarudzbeStavke.Where(x => x.NarudzbaID == n.NarudzbaID).ToList();
                    foreach (var ns in narudzbeStavke.ToList())
                    {
                        IEnumerable<PopustiStavke> popustiStavke = dm.PopustiStavke.Where(x=>x.NarudzbaStavkaID == ns.NarudzbaStavkaID).ToList();
                        foreach (var p in popustiStavke.ToList())
                        {
                            dm.PopustiStavke.Remove(dm.PopustiStavke.Where(x => x.PopustiStavkeID == p.PopustiStavkeID).SingleOrDefault());
                        }
                        dm.NarudzbeStavke.Remove(dm.NarudzbeStavke.Where(x => x.NarudzbaStavkaID
                        == ns.NarudzbaStavkaID).SingleOrDefault());
                    }
                    foreach (var ds in dm.Dostave.Where(x=>x.NarudzbaID == n.NarudzbaID).ToList())
                    {
                        dm.Dostave.Remove(ds);
                    }
                    dm.Narudzbe.Remove(dm.Narudzbe.Where(x => x.NarudzbaID == n.NarudzbaID).SingleOrDefault());
                }

                dm.StavkeMenija.Remove(sm);
            }
            dm.TipoviStavke.Remove(ts);
            dm.SaveChanges();

            return Ok(ts);
        }

        [HttpPut]
        [Route("api/TipoviStavke/{id}")]
        [ResponseType(typeof(void))]
        public IHttpActionResult PutTipoviStavke(int id, TipoviStavke obj)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            TipoviStavke tipoviStavkeOld = dm.TipoviStavke.Find(id);

            tipoviStavkeOld.Naziv = obj.Naziv;
            tipoviStavkeOld.Opis = obj.Opis;

            dm.SaveChanges();

            return Ok();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                dm.Dispose();

            base.Dispose(disposing);
        }
    }
}
