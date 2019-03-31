using eRestoran_API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace eRestoran_API.Controllers
{
    public class ZaposleniciUlogeController : ApiController
    {
        private eRestoranEntities dm = new eRestoranEntities();

        ZaposleniciUlogeController()
        {
            dm.Configuration.LazyLoadingEnabled = false;
        }

        public IHttpActionResult PostZaposleniciUloge(ZaposleniciUloge obj)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            dm.ZaposleniciUloge.Add(obj);
            dm.SaveChanges();
            return CreatedAtRoute("DefaultApi", new { id = obj.ZaposlenikUlogaID }, obj);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                dm.Dispose();

            base.Dispose(disposing);
        }
    }
}
