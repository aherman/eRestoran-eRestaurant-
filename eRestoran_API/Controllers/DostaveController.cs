using eRestoran_API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Data.Entity;

namespace eRestoran_API.Controllers
{
    public class DostaveController : ApiController
    {
        private eRestoranEntities dm = new eRestoranEntities();

        DostaveController()
        {
            dm.Configuration.LazyLoadingEnabled = false;
        }

        [HttpGet]
        [Route("api/Dostave/{id}")]
        public Dostave GetDostave(int id)
        {
            return dm.Dostave.Where(x => x.DostavaID == id).SingleOrDefault();
        }

        [HttpGet]
        [Route("api/Dostave/TrenutneDostave")]
        public List<TrenutneDostave> TrenutneDostave()
        {
            List<TrenutneDostave> prioritetne = dm.Dostave
                .Where(x=>x.IsZavrsena == false && x.Prioritetna == true
                && x.Narudzbe.Aktivna == true)
                .OrderBy(x => x.DatumSlanja)
                .Select(x => new TrenutneDostave
            { 
                dostavaId = x.DostavaID,
                imePrezime = dm.Klijenti.Where(k => k.KlijentID == x.Narudzbe.KlijentID).FirstOrDefault().Ime + " "
                + dm.Klijenti.Where(k => k.KlijentID == x.Narudzbe.KlijentID).FirstOrDefault().Prezime,
                stavke = x.Narudzbe.NarudzbeStavke.Select(s => new TrenutneNarudzbeStavke
                {
                    narudzbaStavkaID = s.NarudzbaStavkaID,
                    naziv = s.StavkeMenija.Naziv,
                    kolicina = s.Kolicina
                }).ToList()
            })
            .ToList();

            List<TrenutneDostave> standardne = dm.Dostave.Where(x => x.IsZavrsena == false && x.Prioritetna == false && x.Narudzbe.Aktivna == true)
                .OrderBy(x => x.DatumSlanja)
                .Select(x => new TrenutneDostave
                {
                    dostavaId = x.DostavaID,
                    imePrezime = dm.Klijenti.Where(k => k.KlijentID == x.Narudzbe.KlijentID).FirstOrDefault().Ime + " "
                + dm.Klijenti.Where(k => k.KlijentID == x.Narudzbe.KlijentID).FirstOrDefault().Prezime,
                    stavke = x.Narudzbe.NarudzbeStavke.Select(s => new TrenutneNarudzbeStavke
                    {
                        narudzbaStavkaID = s.NarudzbaStavkaID,
                        naziv = s.StavkeMenija.Naziv,
                        kolicina = s.Kolicina
                    }).ToList()
                }).ToList();

            List<TrenutneDostave> listDostava = new List<TrenutneDostave>();

            foreach (var item in prioritetne)
            {
                listDostava.Add(item);
            }

            foreach (var item in standardne)
            {
                listDostava.Add(item);
            }

            return listDostava;
        }

        [HttpGet]
        [Route("api/Dostave/DetaljiDostave/{dostavaId}")]
        public DetaljiDostave DetaljiDostave(int dostavaId)
        {
            Dostave temp = dm.Dostave
                .Include(x=>x.Narudzbe)
                .Where(x => x.DostavaID == dostavaId)
                .SingleOrDefault();
            DetaljiDostave detaljiDostave = new DetaljiDostave()
            {
                dostavaId = dostavaId,
                stavke = dm.NarudzbeStavke
                .Include(x=>x.StavkeMenija)
                .Where(x=>x.NarudzbaID == temp.Narudzbe.NarudzbaID).Select(x => new TrenutneNarudzbeStavke
                {
                    narudzbaStavkaID = x.NarudzbaStavkaID,
                    kolicina = x.Kolicina,
                    naziv = x.StavkeMenija.Naziv,
                    cijena = x.StavkeMenija.Cijena * x.Kolicina
                }).ToList(),
                adresaKlijenta = temp.Adresa
            };

            if (!(bool)temp.Narudzbe.IsGotovina)
            {
                detaljiDostave.nacinPlacanja = "Kreditna kartica";
                KreditneKartice kreditnaKartica = dm.KreditneKartice.Where(x => x.KreditnaKarticaID == temp.Narudzbe.KreditnaKarticaID).SingleOrDefault();
                detaljiDostave.kreditnaKartica = new KreditneKarticePrikaz
                {
                    ImePrezime = kreditnaKartica.Ime + " " + kreditnaKartica.Prezime,
                    BrojKartice = kreditnaKartica.BrojKartice
                };
            }
            else
                detaljiDostave.nacinPlacanja = "Gotovina";
                

            return detaljiDostave;
        }

        [HttpPut]
        [Route("api/Dostave/Zavrsi/{dostavaId}")]
        public IHttpActionResult Zavrsi(int dostavaId, Dostave dostava)
        {
            Dostave d = dm.Dostave
                .Include(x=>x.Narudzbe)
                .Where(x => x.DostavaID == dostavaId).SingleOrDefault();

            if (d == null)
                return NotFound();

            d.Narudzbe.IsZavrsena = true;
            d.IsZavrsena = true;
            d.DatumPreuzimanja = DateTime.Now;
            d.Narudzbe.Aktivna = false;
            d.Narudzbe.IsZavrsena = true;
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
