using Klase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Garaza.Presentation
{
    public class OdabirGuma
    {

        public Gume odabraneGume()
        {
            int odabraneGume;

            Console.WriteLine("Izaberite gume: ");
            Console.WriteLine("1. Meke-80km(M)");
            Console.WriteLine("2. Srednjetvrde-100km(S)");
            Console.WriteLine("3. Tvrde-120km(T)");

            while (true)
            {
                Console.Write("Unesite broj: ");
                odabraneGume = Int32.Parse(Console.ReadLine());

                if (odabraneGume > 0 && odabraneGume <= 3)
                {
                    break;

                }
                else
                {
                    Console.WriteLine("Molim Vas unesite jedan od vazecih brojeva");
                }
            }

            GumeRepository gumeRepository = new GumeRepository();

            var GumeOdabrane = gumeRepository.odabraneGume(odabraneGume);

            return GumeOdabrane;
        }

    }
}
