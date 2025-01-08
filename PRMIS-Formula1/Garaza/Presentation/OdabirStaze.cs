using Klase.Models.Staze;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Garaza.Presentation
{
    public class OdabirStaze
    {
        public Staza odabirStaze()
        {
            Console.Write($"Unesite duzinu staze u kilometrima: ");
            int duzinaStaze = Int32.Parse(Console.ReadLine());
            Console.Write($"Unesite osnovno vreme kruga u sekundama: ");
            int vremeKruga = Int32.Parse(Console.ReadLine());

            return new Staza(duzinaStaze, vremeKruga);

        }
    }
}
