using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eRestoran_API.Models
{
    public class StavkeMenijaPrikaz
    {
        public int StavkaMenijaID { get; set; }
        public string Naziv { get; set; }
        public decimal Cijena { get; set; }
        public string Kategorija { get; set; }
        public byte[] Slika { get; set; }
    }
}