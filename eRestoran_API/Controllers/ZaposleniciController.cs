using eRestoran_API.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using System.Data.Entity;

namespace eRestoran_API.Controllers
{
    public class ZaposleniciController : ApiController
    {
        private eRestoranEntities dm = new eRestoranEntities();

        ZaposleniciController()
        {
            dm.Configuration.LazyLoadingEnabled = false;
        }

        [HttpGet]
        [Route("api/Zaposlenici/GetById/{id}")]
        public Zaposlenici GetZaposleniciById(int id)
        {
            return dm.Zaposlenici
                .Include(x=>x.ZaposleniciUloge.Select(y=>y.Uloge))
                .Where(x=>x.ZaposlenikID == id)
                .SingleOrDefault();

        }

        [HttpGet]
        [Route("api/Zaposlenici/ByUsername/{username}")]
        public HttpResponseMessage GetZaposleniciByUsername(string username)
        {
            Zaposlenici z = dm.Zaposlenici
                .Include(x=>x.ZaposleniciUloge.Select(y=>y.Uloge))
                .Where(x => x.KorisnickoIme == username).SingleOrDefault();

            if (z != null)
                return Request.CreateResponse(HttpStatusCode.OK, z);
            else
                return new HttpResponseMessage(HttpStatusCode.NotFound);
        }

        [HttpPost]
        [Route("api/Zaposlenici")]
        public IHttpActionResult PostZaposlenici(Zaposlenici obj)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            
            dm.Zaposlenici.Add(obj);

            foreach (Uloge item in obj.uloge)
            {
                ZaposleniciUloge tempUloga = new ZaposleniciUloge
                {
                    UlogaID = item.UlogaID,
                    DatumIzmjene = DateTime.Now,
                    ZaposlenikID = obj.ZaposlenikID
                };
                dm.ZaposleniciUloge.Add(tempUloga);
                obj.ZaposleniciUloge.Add(tempUloga);
            }

            dm.SaveChanges();


            return Ok();
        }

        [HttpGet]
        [Route("api/Zaposlenici/Nalozi/{uloga}/{imeprezime?}")]
        public List<NaloziZaposlenici> GetZaposleniciNalozi(string uloga = "Sve", string imeprezime = "")
        {
            List<NaloziZaposlenici> lista = dm.Zaposlenici.Select(x => new NaloziZaposlenici
                {
                    zaposlenikID = x.ZaposlenikID,
                    imePrezime = x.Ime + " " + x.Prezime,
                    email = x.Email,
                    username = x.KorisnickoIme
                }).Where(x => x.imePrezime.Contains(imeprezime) || imeprezime == "")
                    .ToList();

            foreach (NaloziZaposlenici item in lista)
            {
                List<ZaposleniciUloge> tempLista = dm.ZaposleniciUloge
                    .Where(x => x.ZaposlenikID == item.zaposlenikID).ToList();
                foreach (ZaposleniciUloge zu in tempLista)
                {
                    item.uloge += " " + dm.Uloge.Where(x=>x.UlogaID == zu.UlogaID).SingleOrDefault().Naziv;
                }
            }
            if (uloga != "Sve")
            {
                List<NaloziZaposlenici> ulogeList = new List<NaloziZaposlenici>();
                foreach (NaloziZaposlenici item in lista)
                {
                    if (item.uloge.Contains(uloga))
                        ulogeList.Add(item);
                }
                lista = ulogeList;
            }

            return lista;
        }

        [HttpDelete]
        [Route("api/Zaposlenici/Obrisi/{id}")]
        public IHttpActionResult DeleteZaposlenici(int id)
        {
            Zaposlenici z = dm.Zaposlenici.Find(id);

            List<ZaposleniciUloge> zaposleniciUloge = dm.ZaposleniciUloge.Where(x => x.ZaposlenikID == id).ToList();
            foreach (ZaposleniciUloge item in zaposleniciUloge)
            {
                dm.ZaposleniciUloge.Remove(item);
            }

            dm.Zaposlenici.Remove(z);
            dm.SaveChanges();

            return Ok();
        }

        [HttpPut]
        [ResponseType(typeof(void))]
        public IHttpActionResult PutZaposlenici(int id, Zaposlenici obj)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            Zaposlenici zaposlenikOld = dm.Zaposlenici.Find(id);

            zaposlenikOld.Ime = obj.Ime;
            zaposlenikOld.Prezime = obj.Prezime;
            zaposlenikOld.Spol = obj.Spol;
            zaposlenikOld.Staz = obj.Staz;
            zaposlenikOld.Telefon = obj.Telefon;
            zaposlenikOld.KorisnickoIme = obj.KorisnickoIme;
            zaposlenikOld.LozinkaSalt = obj.LozinkaSalt;
            zaposlenikOld.LozinkaHash = obj.LozinkaHash;

            List<Uloge> noveUloge = new List<Uloge>(); //nove uloge za dodati

            foreach (var item in zaposlenikOld.ZaposleniciUloge.ToList())
            {
                bool postojeca = false;
                for (int i = obj.uloge.Count()-1; i >=0; i--)
                {
                    if(obj.uloge.ElementAt(i).UlogaID == item.UlogaID)
                    {
                        postojeca = true;
                        obj.uloge.RemoveAt(i);
                    }
                }
                foreach (var uloga in obj.uloge)
                {
                    if (uloga.UlogaID == item.UlogaID)
                        postojeca = true;
                }
                if (postojeca == false) {
                    zaposlenikOld.ZaposleniciUloge.Remove(item);
                    dm.ZaposleniciUloge.Remove(dm.ZaposleniciUloge.Find(item.ZaposlenikUlogaID));
                }
            }

            foreach (Uloge uloga in obj.uloge)
            {
                dm.ZaposleniciUloge.Add(new ZaposleniciUloge
                {
                    UlogaID = uloga.UlogaID,
                    DatumIzmjene = DateTime.Now,
                    ZaposlenikID = obj.ZaposlenikID
                });
            }

                dm.SaveChanges();

            return Ok();
        }

        [HttpGet]
        [Route("api/Zaposlenici/IsDostavljac/{id}")]
        public IHttpActionResult IsDostavljac(int id)
        {
            Zaposlenici z = dm.Zaposlenici.Find(id);
            foreach (var item in dm.ZaposleniciUloge.Where(x=>x.ZaposlenikID == z.ZaposlenikID).ToList())
            {
                if (dm.Uloge.Where(x=>x.UlogaID == item.UlogaID).SingleOrDefault().Naziv == "Dostavljac")
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
