﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eRestoran_PCL.Model
{
    public class TrenutneDostave : ObservableCollection<TrenutneNarudzbeListStavke>
    {
        public int dostavaId { get; set; }
        public string imePrezime { get; set; }
    }
}
