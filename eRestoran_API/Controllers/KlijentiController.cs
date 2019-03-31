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

    public class KlijentiController : ApiController
    {
        private eRestoranEntities dm = new eRestoranEntities();

        KlijentiController()
        {
            dm.Configuration.LazyLoadingEnabled = false;
        }

        [HttpGet]
        [Route("api/Klijenti/GetByParameter/{parameter}")]
        public List<NaloziKlijenti> GetKlijentiParametar(string parameter)
        {
            return dm.Klijenti
            .Select(x => new NaloziKlijenti
            {
                klijentID = x.KlijentID,
                adresa = x.Adresa,
                email = x.Email,
                imePrezime = x.Ime + " " + x.Prezime,
                username = x.KorisnickoIme
            })
            .Where(x => x.imePrezime.Contains(parameter))
            .ToList();
        }

        [HttpGet]
        [Route("api/Klijenti/GetByUsername/{username}")]
        public IHttpActionResult GetKlijentiByUsername(string username)
        {
            dm.Configuration.LazyLoadingEnabled = false;
            Klijenti k = dm.Klijenti.Where(x => x.KorisnickoIme == username).SingleOrDefault();

            if (k == null)
                return NotFound();
            else return Ok(k);
        }

        [HttpGet]
        public List<NaloziKlijenti> GetKlijenti()
        {
            return dm.Klijenti
            .Select(x => new NaloziKlijenti
            {
                klijentID = x.KlijentID,
                adresa = x.Adresa,
                email = x.Email,
                imePrezime = x.Ime + " " + x.Prezime,
                username = x.KorisnickoIme
            })
            .ToList();
        }

        [HttpGet]
        [Route("api/Klijenti/GetById/{id}")]
        public Klijenti GetKlijenti(int id)
        {
            return dm.Klijenti.Where(x => x.KlijentID == id).SingleOrDefault();
        }

        [HttpDelete]
        [Route("api/Klijenti/Obrisi/{id}")]
        public IHttpActionResult DeleteKlijenti(int id)
        {
            Klijenti k = dm.Klijenti.Where(x => x.KlijentID == id).SingleOrDefault();
            List<Narudzbe> narudzbe = dm.Narudzbe
                .Include(x=>x.Dostave)
                .Where(x => x.KlijentID == id).ToList();
            foreach (var n in narudzbe.ToList())
            {
                List<NarudzbeStavke> narudzbeStavke = dm.NarudzbeStavke.Where(x => x.NarudzbaID == n.NarudzbaID).ToList();
                foreach (var ns in narudzbeStavke.ToList())
                {
                    IEnumerable<PopustiStavke> popustiStavke = dm.PopustiStavke.Where(x=>x.NarudzbaStavkaID == ns.NarudzbaStavkaID).ToList();
                    foreach (var p in popustiStavke.ToList())
                    {
                        dm.PopustiStavke.Remove(dm.PopustiStavke.Where(x=>x.PopustiStavkeID == p.PopustiStavkeID).SingleOrDefault());
                    }
                    dm.NarudzbeStavke.Remove(dm.NarudzbeStavke.Where(x=>x.NarudzbaStavkaID
                    == ns.NarudzbaStavkaID).SingleOrDefault());
                }
                foreach (var ds in n.Dostave.ToList())
                {
                    dm.Dostave.Remove(ds);
                }
                dm.Narudzbe.Remove(dm.Narudzbe.Where(x=>x.NarudzbaID == n.NarudzbaID).SingleOrDefault());
            }

            List<KreditneKartice> kreditneKartice = dm.KreditneKartice.Where(x => x.KlijentID == id).ToList();

            foreach(var item in kreditneKartice)
            {
                dm.KreditneKartice.Remove(item);
            }

            List<Ocjene> ocjene = dm.Ocjene.Where(x => x.KlijentID == id).ToList();

            foreach(var item in ocjene)
            {
                dm.Ocjene.Remove(item);
            }

            dm.Klijenti.Remove(k);
            dm.SaveChanges();

            return Ok();
        }

        [HttpPut]
        [Route("api/Klijenti/{id}")]
        [ResponseType(typeof(void))]
        public IHttpActionResult PutKlijenti(int id, Klijenti obj)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            Klijenti klijentiOld = dm.Klijenti.Find(id);
            if (klijentiOld == null)
                klijentiOld = dm.Klijenti.Find(obj.KlijentID);

            if (klijentiOld == null)
                return NotFound();

            klijentiOld.Ime = obj.Ime;
            klijentiOld.Prezime = obj.Prezime;
            klijentiOld.Spol = obj.Spol;
            klijentiOld.Adresa = obj.Adresa;
            klijentiOld.Telefon = obj.Telefon;
            klijentiOld.Email = obj.Email;
            klijentiOld.Narudzbe = obj.Narudzbe;
            klijentiOld.KorisnickoIme = obj.KorisnickoIme;
            klijentiOld.LozinkaSalt = obj.LozinkaSalt;
            klijentiOld.LozinkaHash = obj.LozinkaHash;
            

            dm.SaveChanges();


            return Ok();
        }

        [HttpPost]
        [ResponseType(typeof(Klijenti))]
        public IHttpActionResult PostKlijenti(Klijenti obj)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            dm.Klijenti.Add(obj);

            dm.SaveChanges();

            return Ok(obj);
        }



        protected override void Dispose(bool disposing)
        {
            if (disposing)
                dm.Dispose();

            base.Dispose(disposing);
        }
    }
}
