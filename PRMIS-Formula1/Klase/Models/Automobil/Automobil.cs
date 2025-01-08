using Klase;
using System;

namespace PRMIS_Formula1.Models.Automobil
{
    [Serializable]
    public class Automobil
    {

        public KonfiguracijaAutomobila konfiguracijaAutomobila { get; set; }
        public Gume gumeAutomobila { get; set; }

        public int kolicinaGoriva { get; set; } = 0;

        public int trkackiBroj { get; set; } = 0;

        public Automobil(KonfiguracijaAutomobila konfiguracijaAutomobila, Gume gumeAutomobila)
        {
            this.konfiguracijaAutomobila = konfiguracijaAutomobila;
            this.gumeAutomobila = gumeAutomobila;
        }

        public override string ToString()
        {
            return "\nAutomobil:\n" + konfiguracijaAutomobila.ToString() + gumeAutomobila.ToString() + $"\nKolicina goriva:{kolicinaGoriva} l\n" + $"Trkacki broj: {trkackiBroj} \n";
        }
    }
}
