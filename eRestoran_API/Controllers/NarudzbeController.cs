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
    public class NarudzbeController : ApiController
    {
        private eRestoranEntities dm = new eRestoranEntities();

        NarudzbeController()
        {
            dm.Configuration.LazyLoadingEnabled = false;
        }

        [HttpDelete]
        [Route("api/Narudzbe/{id}")]
        public IHttpActionResult DeleteNarudzbe(int id)
        {
            Narudzbe narudzba = dm.Narudzbe.Find(id);
            int? kreditnaKarticaId = null;

            if (narudzba.KreditnaKarticaID != null)
                kreditnaKarticaId = narudzba.KreditnaKarticaID;

            foreach (var item in dm.NarudzbeStavke.Where(x=>x.NarudzbaID == id).ToList())
            {
                foreach (var popustStavka in dm.PopustiStavke.Where(x => x.NarudzbaStavkaID == item.NarudzbaStavkaID).ToList())
                {
                    dm.PopustiStavke.Remove(popustStavka);
                }
                dm.NarudzbeStavke.Remove(item);
            }

            dm.Dostave.Remove(dm.Dostave.Where(x => x.NarudzbaID == id).SingleOrDefault());

            dm.Narudzbe.Remove(narudzba);

            dm.SaveChanges();

            if (kreditnaKarticaId != null)
            {
                int cardCounter = 0;
                foreach (var item in dm.Narudzbe)
                {
                    if (item.KreditnaKarticaID == kreditnaKarticaId)
                        cardCounter++;
                }

                if(cardCounter == 0)
                {
                    KreditneKartice kartica = dm.KreditneKartice.Find(kreditnaKarticaId);
                    dm.KreditneKartice.Remove(kartica);
                    dm.SaveChanges();
                }
            }

            return Ok();
        }

        [HttpGet]
        [Route("api/Narudzbe/{id}")]
        public NarudzbePrikaz GetNarudzbe(int id)
        {
            Narudzbe x = dm.Narudzbe
                .Include(q => q.Klijenti)
                .Where(n => n.NarudzbaID == id).SingleOrDefault();

            NarudzbePrikaz data = new NarudzbePrikaz()
            {
                narudzbaID = x.NarudzbaID,
                imePrezime = x.Klijenti.Ime + " " + x.Klijenti.Prezime,
                email = x.Klijenti.Email,
                telefon = x.Klijenti.Telefon,
                datumVrijeme = x.Datum.ToString("dd'/'MM'/'yyyy HH:mm")
            };
            data.lokacija = dm.Dostave.Where(d=> d.NarudzbaID == id).SingleOrDefault().Adresa;
            return data;
        }

        [HttpGet]
        [Route("api/Narudzbe/ById/{id}")]
        public NarudzbePost GetNarudzbeById(int id)
        {
            Narudzbe x = dm.Narudzbe.Where(n => n.NarudzbaID == id).SingleOrDefault();

            Dostave d = dm.Dostave.Where(y => y.NarudzbaID == id).SingleOrDefault();

            NarudzbePost model = new NarudzbePost
            {
                KlijentID = x.KlijentID,
                AdresaDostave = d.Adresa,
                IsGotovina = (bool)x.IsGotovina,
                Datum = x.Datum,
                Aktivna = x.Aktivna,
                PrioritetnaDostava = d.Prioritetna,
                NarudzbaID = x.NarudzbaID
            };

            if(x.KreditnaKarticaID != null)
            {
                KreditneKartice tempKartice = dm.KreditneKartice.Where(k => k.KreditnaKarticaID == x.KreditnaKarticaID).SingleOrDefault();
                model.kartica = new KreditneKarticePut {
                    SigurnosniKod = tempKartice.SigurnosniKod,
                    BrojKartice = tempKartice.BrojKartice,
                    GodinaIsteka = tempKartice.GodinaIsteka,
                    Ime = tempKartice.Ime,
                    KlijentID = tempKartice.KlijentID,
                    KreditnaKarticaID = tempKartice.KreditnaKarticaID,
                    MjesecIsteka = tempKartice.MjesecIsteka,
                    Prezime = tempKartice.Prezime
                };
            }

            return model;
        }

        [HttpGet]
        [Route("api/Narudzbe/NarudzbePrikaz")]
        public List<NarudzbePrikaz> GetNarudzbePrikaz()
        {
            List<NarudzbePrikaz> lista = dm.esp_NarudzbePregled()
                .Select(x => new NarudzbePrikaz
            {
                narudzbaID = x.NarudzbaID,
                datumVrijeme = x.Datum.ToString("dd'/'MM'/'yyyy HH:mm"),
                email = x.Email,
                imePrezime = x.imePrezime,
                telefon = x.Telefon,
                ukupnaCijena = Math.Round((decimal)x.ukupnaCijena, 2).ToString() + " KM"
            }).ToList();
            return lista;
        }

        [HttpGet]
        [Route("api/Narudzbe/CijenaByNarudzbaId/{id}")]
        public string CijenaByNarudzbaId(int id)
        {
            decimal sum = 0;
            foreach (var item in dm.NarudzbeStavke.Include(x=>x.StavkeMenija).Where(x=>x.NarudzbaID == id).ToList())
            {
                sum += item.StavkeMenija.Cijena * item.Kolicina;
            }
            return sum.ToString();
        }

        [HttpPut]
        [Route("api/Narudzbe/{id}")]
        public IHttpActionResult Aktivacija(int id, Narudzbe n)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            Narudzbe narudzba = dm.Narudzbe.Find(id);
            if (narudzba == null)
            {
                narudzba = dm.Narudzbe.Where(x => x.NarudzbaID == id).SingleOrDefault();
                if(narudzba == null)
                    return NotFound();
            }  

            narudzba.Aktivna = true;
            Dostave dostava = dm.Dostave.Where(x => x.NarudzbaID == id).SingleOrDefault();
            dostava.DatumSlanja = DateTime.Now;
            dm.SaveChanges();

            return Ok();
        }

        [HttpPut]
        [Route("api/Narudzbe/Uredi/{narudzbaID}")]
        public IHttpActionResult UrediNarudzbe(int narudzbaID, NarudzbePut n)
        {
            bool istaKartica = false;
            Narudzbe narudzba = dm.Narudzbe
                .Include(x=>x.KreditneKartice)
                .Where(x=>x.NarudzbaID == narudzbaID).SingleOrDefault();
            
            if (narudzba == null)
                return NotFound();

            if (n.kartica != null)
            {
                foreach (var item in dm.KreditneKartice.ToList())
                {
                    if(item.BrojKartice == n.kartica.BrojKartice)
                    {
                        if(item.SigurnosniKod == n.kartica.SigurnosniKod)
                        {
                            narudzba.KreditneKartice = item;
                            narudzba.KreditnaKarticaID = item.KreditnaKarticaID;
                            istaKartica = true;
                        }
                        else
                        {
                            return BadRequest();
                        }
                    }
                }
                if (!istaKartica)
                {
                    if (narudzba.KreditneKartice != null)
                    {
                        narudzba.KreditnaKarticaID = null;
                        narudzba.KreditneKartice = null;
                    }
                        

                    KreditneKartice novaKartica = new KreditneKartice();
                    novaKartica.Ime = n.kartica.Ime;
                    novaKartica.Prezime = n.kartica.Prezime;
                    novaKartica.SigurnosniKod = n.kartica.SigurnosniKod;
                    novaKartica.MjesecIsteka = n.kartica.MjesecIsteka;
                    novaKartica.GodinaIsteka = n.kartica.GodinaIsteka;
                    novaKartica.BrojKartice = n.kartica.BrojKartice;
                    novaKartica.KlijentID = narudzba.KlijentID;

                    

                    dm.KreditneKartice.Add(novaKartica);
                    dm.SaveChanges();
                    narudzba.KreditnaKarticaID = novaKartica.KreditnaKarticaID;
                    narudzba.KreditneKartice = novaKartica;
                }
                
            }
            else
            {
                if(narudzba.KreditneKartice != null)
                {
                    narudzba.KreditnaKarticaID = null;
                    narudzba.KreditneKartice = null;
                }  
            }

            narudzba.Datum = DateTime.Now;
            narudzba.IsGotovina = n.IsGotovina;
            
            Dostave dostavaOld = dm.Dostave.Find(narudzbaID);
            dostavaOld.Adresa = n.AdresaDostave;
            dostavaOld.Prioritetna = n.PrioritetnaDostava;

            dm.SaveChanges();
            
            return Ok();

        }

        [HttpGet]
        [Route("api/Narudzbe/TrenutneNarudzbe/{klijentID}")]
        public List<TrenutneNarudzbe> TrenutneNarudzbe(int klijentID)
        {
            return dm.Narudzbe
                .Include(x=>x.NarudzbeStavke)
                .Where(x => x.KlijentID == klijentID && x.IsZavrsena == false)
                .Select(x => new TrenutneNarudzbe
            { 
                narudzbaID = x.NarudzbaID,
                datum = x.Datum,
                aktivna = x.Aktivna,
                stavke = x.NarudzbeStavke.Select(s=>new TrenutneNarudzbeStavke
                {
                    narudzbaStavkaID = s.NarudzbaStavkaID,
                    naziv = dm.StavkeMenija.Where(a=>a.StavkaMenijaID == s.StavkaMenijaID).FirstOrDefault().Naziv,
                    kolicina = s.Kolicina
                }).ToList()
            }).ToList();
        }

        [HttpGet]
        [Route("api/Narudzbe/CijenaByKlijent/{klijentID}")]
        public decimal GetCijenaByKlijent(int klijentID)
        {
            Klijenti k = dm.Klijenti
                .Include(x=>x.Narudzbe)
                .Where(x => x.KlijentID == klijentID).SingleOrDefault();
            if (k == null)
                return 0;

            decimal sum = 0;
            foreach (var narudzba in k.Narudzbe.Where(x=>x.IsZavrsena == true))
            {
                foreach (var stavka in dm.NarudzbeStavke.Where(x=>x.NarudzbaID == narudzba.NarudzbaID).ToList())
                {
                    decimal temp = stavka.Kolicina * dm.StavkeMenija
                        .Where(s=>s.StavkaMenijaID == stavka.StavkaMenijaID).SingleOrDefault().Cijena;
                    foreach (var item in dm.PopustiStavke.Where(x=>x.NarudzbaStavkaID == stavka.NarudzbaStavkaID).ToList())
                    {
                        Popusti p = dm.Popusti.Find(item.PopustID);
                        temp -= temp * (p.Iznos / 100);
                    }
                    sum += temp;
                }
            }
            return sum;
        }

        [HttpGet]
        [Route("api/Narudzbe/CijenaByTipStavke/{tipID}")]
        public decimal GetCijenaByTipStavke(int tipID)
        {
            decimal sum = 0;
            foreach (var narudzba in dm.Narudzbe.Where(x => x.IsZavrsena == true))
            {
                foreach (var stavka in narudzba.NarudzbeStavke.Where(x=>x.StavkeMenija.TipStavkeID == tipID))
                {
                    decimal temp = stavka.Kolicina * stavka.StavkeMenija.Cijena;
                    foreach (var item in dm.PopustiStavke.Where(x => x.NarudzbaStavkaID == stavka.NarudzbaStavkaID).ToList())
                    {
                        Popusti p = dm.Popusti.Find(item.PopustID);
                        temp -= temp * (p.Iznos / 100);
                    }
                    sum += temp;
                }
            }
            return sum;
        }

        //[HttpGet]
        //[Route("api/Narudzbe/CijenaByZaposlenik/{zaposlenikID}")]
        //public decimal GetCijenaByZaposlenik(int zaposlenikID)
        //{
        //    decimal sum = 0;

        //    Zaposlenici z = dm.Zaposlenici.Find(zaposlenikID);

        //    foreach (var item in z.ZaposleniciUloge)
        //    {
        //        if (item.UlogaID == 2)
        //        {
        //            foreach (var narudzba in z.Narudzbe.Where(x=>x.IsZavrsena == true))
        //            {
        //                foreach (var stavka in narudzba.NarudzbeStavke)
        //                {
        //                    decimal temp = stavka.Kolicina * stavka.StavkeMenija.Cijena;
        //                    foreach (var popust in dm.PopustiStavke.Where(x => x.NarudzbaStavkaID == stavka.NarudzbaStavkaID).ToList())
        //                    {
        //                        Popusti p = dm.Popusti.Find(popust.PopustID);
        //                        temp -= temp * (p.Iznos / 100);
        //                    }
        //                    sum += temp;
        //                }
        //            }
        //            return sum;
        //        }
        //        else if(item.UlogaID == 3)
        //        {
        //            foreach (var narudzba in dm.Narudzbe.Where(x => 
        //            dm.Dostave.Where(s => s.NarudzbaID == x.NarudzbaID).FirstOrDefault().ZaposlenikID == zaposlenikID 
        //            && x.IsZavrsena == true))
        //            {
        //                foreach (var stavka in narudzba.NarudzbeStavke)
        //                {
        //                    decimal temp = stavka.Kolicina * stavka.StavkeMenija.Cijena;
        //                    foreach (var popust in dm.PopustiStavke.Where(x => x.NarudzbaStavkaID == stavka.NarudzbaStavkaID).ToList())
        //                    {
        //                        Popusti p = dm.Popusti.Find(popust.PopustID);
        //                        temp -= temp * (p.Iznos / 100);
        //                    }
        //                    sum += temp;
        //                }
        //            }
        //            return sum;
        //        }
        //    }

        //    return 0;
            
        //}

        [HttpGet]
        [Route("api/Narudzbe/PrethodneNarudzbe/{klijentID}")]
        public List<TrenutneNarudzbe> PrethodneNarudzbe(int klijentID)
        {
            return dm.Narudzbe.Where(x => x.KlijentID == klijentID && x.IsZavrsena == true).Select(x => new Models.TrenutneNarudzbe
            {
                narudzbaID = x.NarudzbaID,
                datum = x.Datum,
                stavke = x.NarudzbeStavke.Select(s => new TrenutneNarudzbeStavke
                {
                    narudzbaStavkaID = s.NarudzbaStavkaID,
                    naziv = s.StavkeMenija.Naziv,
                    kolicina = s.Kolicina
                }).ToList()
            }).ToList();
        }

        [HttpPost]
        [Route("api/Narudzbe")]
        public IHttpActionResult PostNarudzbe(NarudzbePost obj)
        {
            if (obj == null)
                return NotFound();

            Narudzbe n = new Narudzbe
            {
                KlijentID = obj.KlijentID,
                Aktivna = false,
                Datum = obj.Datum,
                IsGotovina = obj.IsGotovina
            };

            dm.Narudzbe.Add(n);
            dm.SaveChanges();

            List<Popusti> popusti = new List<Popusti>();

            foreach (var item in dm.Popusti.ToList())
            {
                if (item.DatumPocetka <= DateTime.Now && item.DatumZavrsetka >= DateTime.Now)
                    popusti.Add(item);
            }

            foreach (var stavka in obj.NarudzbeStavke.ToList())
            {
                NarudzbeStavke ns = new NarudzbeStavke
                {
                    Napomena = stavka.Napomena,
                    Kolicina = stavka.Kolicina,
                    StavkaMenijaID = stavka.StavkaMenijaID,
                    NarudzbaID = n.NarudzbaID
                };
                dm.NarudzbeStavke.Add(ns);
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

            dm.SaveChanges();

            dm.Dostave.Add(new Dostave
            {
                Adresa = obj.AdresaDostave,
                IsZavrsena = false,
                Prioritetna = obj.PrioritetnaDostava,
                NarudzbaID = n.NarudzbaID
            });

            if(!obj.IsGotovina)
            {
                KreditneKartice kreditnaKartica;
                if (dm.KreditneKartice.Where(x => x.BrojKartice == obj.kartica.BrojKartice).SingleOrDefault() == null)
                {
                    kreditnaKartica = new KreditneKartice
                    {
                        Ime = obj.kartica.Ime,
                        Prezime = obj.kartica.Prezime,
                        BrojKartice = obj.kartica.BrojKartice,
                        GodinaIsteka = obj.kartica.GodinaIsteka,
                        MjesecIsteka = obj.kartica.MjesecIsteka,
                        KlijentID = obj.KlijentID,
                        SigurnosniKod = obj.kartica.SigurnosniKod
                    };
                    dm.KreditneKartice.Add(kreditnaKartica);
                    dm.SaveChanges();
                }
                else
                {
                    kreditnaKartica = dm.KreditneKartice.Where(x => x.BrojKartice == obj.kartica.BrojKartice).SingleOrDefault();
                }
                n.KreditnaKarticaID = kreditnaKartica.KreditnaKarticaID;
            }

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
