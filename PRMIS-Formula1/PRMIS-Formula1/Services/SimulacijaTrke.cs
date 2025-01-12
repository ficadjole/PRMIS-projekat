using Klase.Models.Staze;
using PRMIS_Formula1.Models.Automobil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PRMIS_Formula1.Services
{
    public class SimulacijaTrke
    {

        public List<double> simulacija(ref Automobil automobil, Staza staza) {


            int n = 0;
            List<double> vremenaPoKrugu = new List<double>();

            do
            {
                automobil.gumeAutomobila.duzinaKoriscenja = automobil.gumeAutomobila.duzinaKoriscenja - (int)(staza.duzinaStaze * automobil.konfiguracijaAutomobila.potrosnjaGuma);
                automobil.kolicinaGoriva = automobil.kolicinaGoriva - (int)(staza.duzinaStaze * automobil.konfiguracijaAutomobila.potrosnjaGoriva);

                double tempoGuma = 0, tempoGoriva;

                tempoGoriva = 1 / automobil.kolicinaGoriva;

                if (automobil.gumeAutomobila.duzinaKoriscenja < automobil.gumeAutomobila.duzinaKoriscenja * 0.4)
                {
                    tempoGuma = 0.6 * (n + 1);
                }
                else
                {
                    switch (automobil.gumeAutomobila.gumaType)
                    {
                        case Klase.GumaType.M:
                            tempoGuma = 1.2 * (n + 1);
                            break;

                        case Klase.GumaType.T:
                            tempoGuma = 0.8 * (n + 1);
                            break;

                        case Klase.GumaType.S:
                            tempoGuma = n + 1;
                            break;
                    }
                }

                double vremeKruga = staza.vremeTrajanjaKruga - tempoGoriva - tempoGuma;

                vremenaPoKrugu.Add(vremeKruga);

                Thread.Sleep((int)vremeKruga);

                Console.Write(vremeKruga + " ");

                n++;

            } while (n < 10);

            return vremenaPoKrugu;

        } 

    }
}
