using Klase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRMIS_Formula1.Models.Automobil
{
    public class Automobil
    {

        public KonfiguracijaAutomobila konfiguracijaAutomobila {  get; set; }
        public Gume gumeAutomobila { get; set; }

        public Automobil(KonfiguracijaAutomobila konfiguracijaAutomobila, Gume gumeAutomobila)
        {
            this.konfiguracijaAutomobila = konfiguracijaAutomobila;
            this.gumeAutomobila = gumeAutomobila;
        }

        public override string ToString()
        {
            return "\nAutomobil:\n"+konfiguracijaAutomobila.ToString() + gumeAutomobila.ToString() + "\n";
        }
    }
}
