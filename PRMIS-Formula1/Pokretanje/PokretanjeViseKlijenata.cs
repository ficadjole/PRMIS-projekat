using System;
using System.Diagnostics;

namespace PRMIS_Formula1.Services
{
    public class PokretanjeViseKlijenata
    {

        public void PokreniKlijente()
        {
            for (int i = 0; i < 2; i++)
            {

                string automobilClient = @"C:\Users\Filip\OneDrive\Desktop\PRMIS-projekat\PRMIS-Formula1\PRMIS-Formula1\bin\Debug\PRMIS-Formula1.exe";

                Process automobilProces = new Process();

                automobilProces.StartInfo.FileName = automobilClient;
                automobilProces.StartInfo.Arguments = $"{i + 1}";
                automobilProces.Start();


                Console.WriteLine($"Pokernut je klijent #{i + 1}");

            }




        }

    }
}
