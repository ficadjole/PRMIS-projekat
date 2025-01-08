using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Klase
{
    public class GumeRepository
    {
        public List<Gume> gumaList { get; set; } = new List<Gume>() { new Gume(GumaType.M,80),
                                                                      new Gume(GumaType.S,100),
                                                                      new Gume(GumaType.T,120)};

        public Gume odabraneGume(int brOdabrane)
        {
            return gumaList[brOdabrane - 1];
        }
    }
}
