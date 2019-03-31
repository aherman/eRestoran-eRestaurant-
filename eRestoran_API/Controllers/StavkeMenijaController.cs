using eRestoran_API.Models;
using eRestoran_API.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace eRestoran_API.Controllers
{
    public class StavkeMenijaController : ApiController
    {
        private eRestoranEntities dm = new eRestoranEntities();

        StavkeMenijaController()
        {
            dm.Configuration.LazyLoadingEnabled = false;
        }

        [Route("api/StavkeMenija/{kategorija}")]
        public List<StavkeMenijaPrikaz> GetStavkeMenija(string kategorija)
        {
            List<StavkeMenijaPrikaz> lista = null;

            if (kategorija == "Sve")
            {
                lista = dm.StavkeMenija
                .Include("TipoviStavke")
                .Select(x => new StavkeMenijaPrikaz
                {
                    StavkaMenijaID = x.StavkaMenijaID,
                    Cijena = x.Cijena,
                    Kategorija = x.TipoviStavke.Naziv,
                    Naziv = x.Naziv,
                    Slika = x.SlikaThumb
                }).ToList();
            }
            else
            {
                lista = dm.StavkeMenija
                    .Where(x => x.TipoviStavke.Naziv == kategorija)
                .Select(x => new StavkeMenijaPrikaz
                {
                    StavkaMenijaID = x.StavkaMenijaID,
                    Cijena = x.Cijena,
                    Kategorija = x.TipoviStavke.Naziv,
                    Naziv = x.Naziv,
                    Slika = x.SlikaThumb
                }).ToList();
            }
            return lista;
        }

        [HttpGet]
        [Route("api/StavkeMenija/Recommender/{stavkaId}")]
        public List<StavkeMenijaPrikaz> Recommender(int stavkaId)
        {
            Recommender r = new Recommender();
            List<StavkeMenijaPrikaz> lista = r.GetSimilarProducts(stavkaId).Select(x => new StavkeMenijaPrikaz
            {
                StavkaMenijaID = x.StavkaMenijaID,
                Cijena = x.Cijena,
                Slika = x.SlikaThumb,
                Naziv = x.Naziv
            }).Take(3).ToList();
            return lista;
        }

        [Route("api/StavkeMenija/GetByNaziv/{naziv}")]
        public List<StavkeMain> GetStavkeMenija2(string naziv)
        {
            List<StavkeMenija> lista = null;

            if (naziv == "Sve")
            {
                lista = dm.StavkeMenija.ToList();
            }
            else
            {
                lista = dm.StavkeMenija
                    .Where(x => x.Naziv.Contains(naziv)).ToList();
            }
            return lista.Select(x=>new StavkeMain {
                Cijena = x.Cijena,
                Sifra = x.Sifra,
                SlikaThumb = x.SlikaThumb,
                Status = x.Status,
                StavkaMenijaID = x.StavkaMenijaID,
                TipStavkeID = x.TipStavkeID,
                Naziv = x.Naziv,
                Opis = x.Opis
            }).ToList();
        }

        [HttpGet]
        [Route("api/StavkeMenija/GetById/{id}")]
        public StavkeMenija GetStavkeMenija(int id)
        {
            return dm.StavkeMenija.Where(x => x.StavkaMenijaID == id).SingleOrDefault();
        }

        [HttpGet]
        [Route("api/StavkeMenija/ById/{id}")]
        public StavkeMenijaEditPrikaz GetStavkeMenijaEdit(int id)
        {
            StavkeMenija sm = dm.StavkeMenija.Find(id);
            return new StavkeMenijaEditPrikaz
            {
                StavkaMenijaID = sm.StavkaMenijaID,
                cijena = sm.Cijena,
                naziv = sm.Naziv
            };
        }

        [HttpGet]
        [Route("api/StavkeMenija/GetAll")]
        public List<StavkeMain> GetStavkeMenijaAll()
        {
            return dm.StavkeMenija.Select(x => new StavkeMain
            {
                Cijena = x.Cijena,
                Sifra = x.Sifra,
                SlikaThumb = x.SlikaThumb,
                Status = x.Status,
                StavkaMenijaID = x.StavkaMenijaID,
                TipStavkeID = x.TipStavkeID,
                Naziv = x.Naziv,
                Opis = x.Opis
            }).ToList();
        }

        [Route("DELETE")]
        [Route("api/StavkeMenija/Obrisi/{id}")]
        public IHttpActionResult DeleteStavkeMenija(int id)
        {
            StavkeMenija sm = dm.StavkeMenija.Find(id);

            foreach (var item in dm.Ocjene.Where(x=>x.StavkaMenijaID == id).ToList())
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
            dm.SaveChanges();

            return Ok();
        }

        [HttpPost]
        public IHttpActionResult PostStavkeMenija(StavkeMenija obj)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            dm.StavkeMenija.Add(obj);

            dm.SaveChanges();


            return CreatedAtRoute("DefaultApi", new { id = obj.StavkaMenijaID }, obj);
        }

        [HttpPut]
        [Route("api/StavkeMenija/{id}")]
        [ResponseType(typeof(void))]
        public IHttpActionResult PutStavkeMenija(int id, StavkeMenija obj)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            
            StavkeMenija stavkeMenijaOld = dm.StavkeMenija.Find(id);


            stavkeMenijaOld.Cijena = obj.Cijena;
            stavkeMenijaOld.Naziv = obj.Naziv;
            stavkeMenijaOld.Opis = obj.Opis;
            stavkeMenijaOld.Sifra = obj.Sifra;
            stavkeMenijaOld.Slika = obj.Slika;
            stavkeMenijaOld.SlikaThumb = obj.SlikaThumb;
            stavkeMenijaOld.Status = obj.Status;
            stavkeMenijaOld.TipStavkeID = obj.TipStavkeID;
            
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
