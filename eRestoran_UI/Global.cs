using eRestoran_API.Models;
using eRestoran_UI.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace eRestoran_UI
{
    class Global
    {
        public static Zaposlenici prijavljeniZaposlenik { get; set; }


        public static bool IsAdmin()
        {
            foreach (var item in prijavljeniZaposlenik.ZaposleniciUloge)
            {
                if (item.Uloge.Naziv == "Administrator")
                    return true;
            }
            return false;
        }

        public static bool IsDostavljac()
        {
            foreach (var item in prijavljeniZaposlenik.ZaposleniciUloge)
            {
                if (item.Uloge.Naziv == "Dostavljac")
                    return true;
            }
            return false;
        }

        public static bool IsOperater()
        {
            foreach (var item in prijavljeniZaposlenik.ZaposleniciUloge)
            {
                if (item.Uloge.Naziv == "Operater")
                    return true;
            }
            return false;
        }
    }
}
