//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace eRestoran_API.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Dostave
    {
        public int DostavaID { get; set; }
        public int NarudzbaID { get; set; }
        public bool IsZavrsena { get; set; }
        public bool Prioritetna { get; set; }
        public Nullable<System.DateTime> DatumSlanja { get; set; }
        public Nullable<System.DateTime> DatumPreuzimanja { get; set; }
        public string Adresa { get; set; }
    
        public virtual Narudzbe Narudzbe { get; set; }
    }
}
