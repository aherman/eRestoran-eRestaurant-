using eRestoran_API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace eRestoran_API.Controllers
{
    public class UlogeController : ApiController
    {
        private eRestoranEntities dm = new eRestoranEntities();

        UlogeController()
        {
            dm.Configuration.LazyLoadingEnabled = false;
        }

        public List<Uloge> GetKlijenti()
        {
            return dm.Uloge.ToList();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                dm.Dispose();

            base.Dispose(disposing);
        }
    }
}
