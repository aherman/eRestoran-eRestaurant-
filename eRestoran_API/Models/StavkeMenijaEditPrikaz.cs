using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eRestoran_API.Models
{
    public class StavkeMenijaEditPrikaz
    {
        public int StavkaMenijaID { get; set; }
        public string naziv { get; set; }
        public decimal cijena { get; set; }
    }
}