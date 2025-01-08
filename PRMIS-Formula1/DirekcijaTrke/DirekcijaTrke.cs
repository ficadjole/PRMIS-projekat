using DirekcijaTrke.Services.PokretanjeServeraServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace DirekcijaTrke
{
    internal class DirekcijaTrke
    {
        static void Main(string[] args)
        {
            Console.WriteLine("--------------- Direkcija Trke ---------------");

            PokretanjeServeraService pokretanje = new PokretanjeServeraService();

            pokretanje.PokretanjeServera();

        }
    }
}
