using Klase;
using PRMIS_Formula1.Models.Automobil;
using System;

namespace PRMIS_Formula1.Presentation
{
    public class ParsiranjePorukeGaraze
    {

        public void parsiranjePorukeGaraze(string poruka, ref Automobil automobil)
        {
            string[] deo = poruka.Split(':');

            string[] deo2 = deo[1].Split(',');

            switch ((deo2[0].ToString()).Trim())
            {
                case "M":
                    automobil.gumeAutomobila.gumaType = GumaType.M;
                    automobil.gumeAutomobila.duzinaKoriscenja = 80;
                    break;
                case "S":
                    automobil.gumeAutomobila.gumaType = GumaType.S;
                    automobil.gumeAutomobila.duzinaKoriscenja = 100;
                    break;
                case "T":
                    automobil.gumeAutomobila.gumaType = GumaType.T;
                    automobil.gumeAutomobila.duzinaKoriscenja = 120;
                    break;
            }

            automobil.kolicinaGoriva = Int32.Parse(deo2[1]);
        }

    }
}
