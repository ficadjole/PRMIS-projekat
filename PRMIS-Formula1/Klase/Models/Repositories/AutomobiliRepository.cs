using System.Collections.Generic;

namespace Klase
{
    public class AutomobiliRepository
    {

        public List<KonfiguracijaAutomobila> AutomobilaList { get; set; } = new List<KonfiguracijaAutomobila> { new KonfiguracijaAutomobila("Mercedes",0.3,0.6),
                                                                                                                new KonfiguracijaAutomobila("Ferari",0.3,0.5),
                                                                                                                new KonfiguracijaAutomobila("Reno",0.4,0.7),
                                                                                                                new KonfiguracijaAutomobila("Honda",0.2,0.6)};

        public KonfiguracijaAutomobila OdabranAuto(int odabir)
        {
            return AutomobilaList[odabir - 1];
        }

    }
}
