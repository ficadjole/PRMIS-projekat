using Klase;
using PRMIS_Formula1.Models.Automobil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRMIS_Formula1.Presentation
{
    public class OdabirKonfiguracije
    {

        public Automobil odabirKonfiugracijeIGuma()
        {
            Console.WriteLine("Odaberite proizvodjaca automobila: ");
            Console.WriteLine("1. Mercedes");
            Console.WriteLine("2. Ferari");
            Console.WriteLine("3. Reno");
            Console.WriteLine("4. Honda");

            int unos;

            while (true)
            {
                Console.Write("Unesite broj: ");
                unos = Int32.Parse(Console.ReadLine());

                if (unos > 0 && unos <= 4)
                {
                    break;

                }
                else
                {
                    Console.WriteLine("Molim Vas unesite jedan od vazecih brojeva");
                }
            }

            AutomobiliRepository repo = new AutomobiliRepository();

            var odabranaKonfiguracija = repo.OdabranAuto(unos);

            return new Automobil(odabranaKonfiguracija, new Gume());
        }

    }
}
