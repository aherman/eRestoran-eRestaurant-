using eRestoran_API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eRestoran_API.Util
{
    public class Recommender
    {
        private eRestoranEntities dm = new eRestoranEntities();
        private Dictionary<int, List<Ocjene>> stavkeMenija = new Dictionary<int, List<Ocjene>>();

        public List<StavkeMenija> GetSimilarProducts(int trenutnaStavkaMenijaId)
        {

            getStavkeMenija(trenutnaStavkaMenijaId);

            List<Ocjene> ocjeneTrenutneStavkeMenija = dm.Ocjene.Where(x => x.StavkaMenijaID == trenutnaStavkaMenijaId).OrderBy(x => x.KlijentID).ToList();

            List<Ocjene> commonRatings1 = new List<Ocjene>();
            List<Ocjene> commonRatings2 = new List<Ocjene>();

            List<StavkeMenija> slicneStavkeMenija = new List<StavkeMenija>();

            foreach (var item in stavkeMenija)
            {
                foreach (Ocjene o in ocjeneTrenutneStavkeMenija)
                {
                    if (item.Value.Where(x => x.KlijentID == o.KlijentID).Count() > 0)
                    {
                        commonRatings1.Add(o);
                        commonRatings2.Add(item.Value.Where(x => x.KlijentID == o.KlijentID).FirstOrDefault());
                    }
                }

                double slicnost = izracunajSlicnost(commonRatings1, commonRatings2);

                if (slicnost > 0.7)
                {
                    slicneStavkeMenija.Add(dm.StavkeMenija.Where(x=>x.StavkaMenijaID == item.Key).SingleOrDefault());
                }

                commonRatings1.Clear();
                commonRatings2.Clear();
            }

            if(slicneStavkeMenija.Count < 3)//cold start
            {
                List<NajcesciProizvodi> proizvodiID = dm.esp_NajcesciProizvodi().Select(x => new NajcesciProizvodi
                {
                    id = x.id,
                    naziv = x.naziv
                }).ToList();

                int count = proizvodiID.Count;

                    for (int i = 1; i < count; i++)
                    {
                        int tempID = i;
                        if (proizvodiID[tempID].id != trenutnaStavkaMenijaId)
                        {
                            bool postojiUListi = false;
                            int tempNum = proizvodiID[tempID].id;
                            StavkeMenija tempStavka = dm.StavkeMenija.Where(x => x.StavkaMenijaID == tempNum).SingleOrDefault();
                            foreach (var item in slicneStavkeMenija.ToList())
                            {
                                if(item.StavkaMenijaID == tempStavka.StavkaMenijaID)
                                {
                                    postojiUListi = true;
                                }
                            }
                            if(!postojiUListi)
                                slicneStavkeMenija.Add(tempStavka);
                        }
                            
                        if (slicneStavkeMenija.Count == 3)
                            break;
                    }
            }

            return slicneStavkeMenija;
        }

        

        private void getStavkeMenija(int stavkaMenijaId)
        {
            List<StavkeMenija> aktivneStavke = dm.StavkeMenija.Where(x => x.StavkaMenijaID != stavkaMenijaId && x.Status == true).ToList();

            List<Ocjene> ocjene;
            foreach (var item in aktivneStavke)
            {
                ocjene = dm.Ocjene.Where(x => x.StavkaMenijaID == item.StavkaMenijaID).OrderBy(x => x.KlijentID).ToList();

                if (ocjene.Count > 0)
                {
                    stavkeMenija.Add(item.StavkaMenijaID, ocjene);
                }
            }
        }

        private double izracunajSlicnost(List<Ocjene> ocjene1, List<Ocjene> ocjene2)
        {
            if (ocjene1.Count != ocjene2.Count)
                return 0;

            double numerator = 0, int1 = 0, int2 = 0;

            for (int i = 0; i < ocjene1.Count; i++)
            {
                numerator += ocjene1[i].Ocjena * ocjene2[i].Ocjena;
                int1 += Math.Pow(ocjene1[i].Ocjena, 2);
                int2 += Math.Pow(ocjene2[i].Ocjena, 2);

            }

            int1 = Math.Sqrt(int1);
            int2 = Math.Sqrt(int2);

            if (int1 * int2 != 0)
                return numerator / (int1 * int2);

            return 0;
        }
    }
}