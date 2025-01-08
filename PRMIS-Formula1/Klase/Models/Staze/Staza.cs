using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Klase.Models.Staze
{
    public class Staza
    {

        public int duzinaStaze {  get; set; }
        public int vremeTrajanjaKruga { get; set; }

        public Staza() { }

        public Staza(int duzinaStaze, int vremeTrajanjaKruga)
        {
            this.duzinaStaze = duzinaStaze;
            this.vremeTrajanjaKruga = vremeTrajanjaKruga;
        }

        public override string ToString()
        {
            return "\nStaza: \n"+$"Duzina staze: {duzinaStaze}km\n"+$"Vreme kruga: {vremeTrajanjaKruga}s\n";
        }
    }
}
