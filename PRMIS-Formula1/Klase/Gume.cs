﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Klase
{
    public class Gume
    {

        public GumaType gumaType { get; set; }
        public int duzinaKoriscenja { get; set; }

        

        public Gume() { }

        public Gume (GumaType gumaType, int duzinaKoriscenja)
        {
            this.gumaType = gumaType;
            this.duzinaKoriscenja = duzinaKoriscenja;
        }


        public override string ToString()
        {
            string s = "Tip gume: ";

            switch (gumaType)
            {
                case GumaType.M: 
                    s += "Meke ";
                break;
                case GumaType.S:
                    s += "Srednje tvrde ";
                break;
                case GumaType.T:
                    s += "Tvrde ";
                break;
            }

            return s+= $"\tDuzina koriscenja:{duzinaKoriscenja}";
        }
    }
}
