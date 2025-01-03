using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Klase
{
    [Serializable]
    public class Rezultati
    {

        public string Kljuc { get; set; }
        public List<double> Vremena { get; set; }

    }
}
