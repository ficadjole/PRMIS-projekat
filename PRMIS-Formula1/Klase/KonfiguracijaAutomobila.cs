using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Klase
{
    public class KonfiguracijaAutomobila
    {

        public string ime {  get; set; }    
        public double potrosnjaGuma {  get; set; }
        public double potrosnjaGoriva { get; set; }
        //public List<KonfiguracijaAutomobila> AutomobilaList { get; set; } = new List<KonfiguracijaAutomobila> { new KonfiguracijaAutomobila("Mercedes",0.3,0.6),
        //                                                                                                        new KonfiguracijaAutomobila("Ferari",0.3,0.5),
        //                                                                                                        new KonfiguracijaAutomobila("Reno",0.4,0.7),
        //                                                                                                        new KonfiguracijaAutomobila("Honda",0.2,0.6)};
        //public KonfiguracijaAutomobila() { }

        public KonfiguracijaAutomobila(string ime, double potrosnjaGuma, double potrosnjaGoriva)
        {
            this.ime = ime;
            this.potrosnjaGuma = potrosnjaGuma;
            this.potrosnjaGoriva = potrosnjaGoriva;
        }

        //public KonfiguracijaAutomobila OdabranAuto(int odabir)
        //{
        //    return AutomobilaList[odabir - 1];
        //}

        public override string ToString()
        {
            return $"Ime: {ime}\nPotrosnja guma: {potrosnjaGuma}\nPotrosnja goriva: {potrosnjaGoriva}\n";
        }
    }
}
