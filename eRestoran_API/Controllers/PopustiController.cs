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
    public class PopustiController : ApiController
    {
        private eRestoranEntities dm = new eRestoranEntities();

        PopustiController()
        {
            dm.Configuration.LazyLoadingEnabled = false;
        }

        [HttpGet]
        [Route("api/Popusti")]
        public IHttpActionResult GetPopusti()
        {
            if (dm.Popusti.ToList().Count == 0)
                return NotFound();
            return Ok(dm.Popusti.ToList());
        }

        [HttpGet]
        [Route("api/Popusti/ImaPopust/{narudzbaId}")]
        public List<PopustiModel> GetImaPopust(int narudzbaId)
        {
            List<PopustiModel> temp = new List<PopustiModel>();
            List<NarudzbeStavke> ns = dm.NarudzbeStavke
                .Include(x => x.PopustiStavke)
                .Where(x => x.NarudzbaID == narudzbaId)
                .ToList();
            foreach (var item in ns)
            {
                if(item.PopustiStavke.Count != 0)
                {
                    foreach (var popustStavka in dm.PopustiStavke
                        .Where(x=>x.NarudzbaStavkaID == item.NarudzbaStavkaID).ToList())
                    {
                        Popusti tempPopust = dm.Popusti.Where(x => x.PopustID == popustStavka.PopustID).SingleOrDefault();
                        PopustiModel popustiModel = new PopustiModel(){
                            popustID = tempPopust.PopustID,
                            iznos = tempPopust.Iznos
                        };
                        temp.Add(popustiModel);
                    }
                }
                return temp;
            }
            return temp;
        }

        [HttpGet]
        [Route("api/Popusti/TrenutniPopusti")]
        public IHttpActionResult GetTrenutniPopusti()
        {
            if (dm.Popusti.ToList().Count == 0)
                return NotFound();

            List<Popusti> popustiList = new List<Popusti>();

            foreach (var item in dm.Popusti)
            {
                if (item.DatumPocetka <= DateTime.Now && item.DatumZavrsetka >= DateTime.Now)
                    popustiList.Add(item);
            }

            return Ok(popustiList);
        }

        [HttpGet]
        [Route("api/Popusti/GetById/{id}")]
        public Popusti GetPopustiId(int id)
        {
            return dm.Popusti.Find(id);
        }

        [HttpPost]
        [Route("api/Popusti")]
        public IHttpActionResult PostPopusti(Popusti obj)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            dm.Popusti.Add(obj);
            dm.SaveChanges();

            return Ok(obj);
        }

        [HttpDelete]
        [Route("api/Popusti/Obrisi/{id}")]
        public IHttpActionResult DeletePopusti(int id)
        {
            foreach (var item in dm.PopustiStavke.Where(x=>x.PopustID == id).ToList())
            {
                dm.PopustiStavke.Remove(item);
            }
            Popusti obj = dm.Popusti.Find(id);

            dm.Popusti.Remove(obj);
            dm.SaveChanges();

            return Ok(obj);
        }

        [HttpPut]
        [Route("api/Popusti/{id}")]
        [ResponseType(typeof(void))]
        public IHttpActionResult PutPopusti(int id, Popusti obj)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            Popusti popustiOld = dm.Popusti.Find(id);

            popustiOld.Naziv = obj.Naziv;
            popustiOld.Opis = obj.Opis;
            popustiOld.DatumPocetka = obj.DatumPocetka;
            popustiOld.DatumZavrsetka = obj.DatumZavrsetka;
            popustiOld.Iznos = obj.Iznos;

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
