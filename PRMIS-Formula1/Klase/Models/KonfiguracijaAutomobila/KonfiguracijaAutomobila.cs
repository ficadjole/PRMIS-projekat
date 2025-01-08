﻿using System;
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

        public KonfiguracijaAutomobila(string ime, double potrosnjaGuma, double potrosnjaGoriva)
        {
            this.ime = ime;
            this.potrosnjaGuma = potrosnjaGuma;
            this.potrosnjaGoriva = potrosnjaGoriva;
        }

        public override string ToString()
        {
            return $"Ime: {ime}\nPotrosnja guma: {potrosnjaGuma}\nPotrosnja goriva: {potrosnjaGoriva} l/km\n";
        }
    }
}