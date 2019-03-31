using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web.Http;
using eRestoran_API.Models;


namespace eRestoran_API.Controllers
{

    public class NarudzbeStavkeController : ApiController
    {
        private eRestoranEntities dm = new eRestoranEntities();

        NarudzbeStavkeController()
        {
            dm.Configuration.LazyLoadingEnabled = false;
        }

        [Route("api/NarudzbeStavke/GetByNarudzba/{narudzbaID}")]
        public List<NarudzbeStavkePrikaz> GetNarudzbeStavke(int narudzbaID)
        {
            List<NarudzbeStavkePrikaz> lista = dm.NarudzbeStavke
                .Where(x => x.NarudzbaID == narudzbaID)
                .Select(x => new NarudzbeStavkePrikaz
                {
                    narudzbaStavkaID = x.NarudzbaStavkaID,
                    cijena = Math.Round(x.StavkeMenija.Cijena, 2).ToString() + " KM",
                    kolicina = x.Kolicina,
                    naziv = x.StavkeMenija.Naziv,
                    napomena = x.Napomena,
                    ukupnaCijena = (x.StavkeMenija.Cijena * x.Kolicina).ToString()
                }).ToList();

            foreach (var item in lista)
            {
                decimal temp = Math.Round(Convert.ToDecimal(item.ukupnaCijena),2);
                item.ukupnaCijena = temp.ToString() + " KM";
            }

            return lista;
        }

        [HttpGet]
        [Route("api/NarudzbeStavke/{narudzbaID}")]
        public List<NarudzbeStavkeEdit> GetNaruzdbeStavkeByNaruzba(int narudzbaID)
        {
            List<NarudzbeStavke> stavke = dm.NarudzbeStavke.Where(x => x.NarudzbaID == narudzbaID).ToList();

            return stavke.Select(x => new NarudzbeStavkeEdit
            {
                StavkaMenijaID = x.StavkaMenijaID,
                NarudzbaStavkaID = x.NarudzbaStavkaID,
                Kolicina = x.Kolicina,
                Napomena = x.Napomena,
                NarudzbaID = x.NarudzbaID
            }).ToList();
        }

        [HttpPut]
        [Route("api/NarudzbeStavke/Uredi/{narudzbaID}")]
        public IHttpActionResult PutNarudzbeStavke(int narudzbaID, List<NarudzbeStavkeEdit> obj)
        {
            Narudzbe narudzba = dm.Narudzbe
                .Include(r=>r.NarudzbeStavke)
                .Where(x => x.NarudzbaID == narudzbaID).SingleOrDefault();

            try
            {
                List<NarudzbeStavke> tempStavke = narudzba.NarudzbeStavke.ToList();
                foreach (var item in tempStavke)
                {
                    foreach (var popust in dm.PopustiStavke.Where(x=>x.NarudzbaStavkaID == item.NarudzbaStavkaID).ToList())
                    {
                        dm.PopustiStavke.Remove(popust);
                    }
                    dm.NarudzbeStavke.Remove(item);
                }

                narudzba.NarudzbeStavke = new List<NarudzbeStavke>();

                dm.SaveChanges();
            }
            catch (Exception e)
            {
                return NotFound();
                throw;
            }


            List<Popusti> popusti = new List<Popusti>();

            foreach (var item in dm.Popusti.ToList())
            {
                if (item.DatumPocetka <= DateTime.Now && item.DatumZavrsetka >= DateTime.Now)
                    popusti.Add(item);
            }


            try
            {

                foreach (var stavka in obj.ToList())
                {
                    NarudzbeStavke ns = new NarudzbeStavke
                    {
                        Napomena = stavka.Napomena,
                        Kolicina = stavka.Kolicina,
                        StavkaMenijaID = stavka.StavkaMenijaID,
                        NarudzbaID = narudzbaID
                    };
                    dm.NarudzbeStavke.Add(ns);
                    narudzba.NarudzbeStavke.Add(ns);
                    dm.SaveChanges();
                    foreach (var popust in popusti.ToList())
                    {
                        PopustiStavke ps = new PopustiStavke
                        {
                            NarudzbaStavkaID = ns.NarudzbaStavkaID,
                            PopustID = popust.PopustID
                        };
                        dm.PopustiStavke.Add(ps);
                    }

                }
            }
            catch (Exception exception)
            {
                return BadRequest();
                throw;
            }


            try
            {
                dm.SaveChanges();
            }
            catch (Exception exception)
            {
                return StatusCode(System.Net.HttpStatusCode.Conflict);
                throw;
            }
            

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
