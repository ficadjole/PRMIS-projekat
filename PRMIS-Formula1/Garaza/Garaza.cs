using Garaza.Presentation;
using Klase;
using Klase.Models.Staze;
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace Garaza
{
    internal class Garaza
    {
        static void Main(string[] args)
        {

            Console.WriteLine("--------------- GARAZA ---------------");


            //KREIRANJE TCP UTICNICE I TCP SERVERA - preko toga se salju podaci od automobila ka garazi

            Socket garazaTCPSoket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            Console.WriteLine($"Podaci TCP uticnice: {garazaTCPSoket.AddressFamily}:{garazaTCPSoket.SocketType}:{garazaTCPSoket.ProtocolType}");

            EndPoint garazaTCPPoint = new IPEndPoint(IPAddress.Any, 53003);

            garazaTCPSoket.Bind(garazaTCPPoint);

            garazaTCPSoket.Listen(20);


            //KREIRANJE UDP UTICNICE - preko nje garaza salje podatke automobilu

            Socket garazaUDPSoket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            
            EndPoint garazaUDPPoint = new IPEndPoint(IPAddress.Parse("192.168.0.34"), 53002);

            Staza odabranaStaza = new OdabirStaze().odabirStaze();

            //ODABIR STAZE, GUMA i GORIVA

            Console.WriteLine(odabranaStaza.ToString());

            Gume odabraneGume = new OdabirGuma().odabraneGume();

            Console.WriteLine("Unesite kolicinu goriva koja se nalazi u kolima: ");

            int kolicinaGoriva = Int32.Parse(Console.ReadLine());

            Console.WriteLine(odabraneGume.ToString());

            //PARSIRANJE PORUKE ZA SLANJE AUTOMOBILU

            string poruka = "Izlazak na stazu: ";

            switch (odabraneGume.gumaType)
            {
                case GumaType.M:
                    poruka += " M,";
                    break;
                case GumaType.S:
                    poruka += " S,";
                    break;
                case GumaType.T:
                    poruka += " T,";
                    break;
            }

            poruka += $"{kolicinaGoriva}";

            //SLANJE PORUKE AUTOMOBILU O GUMAMA I GORIVU

            byte[] binarnaPoruka = Encoding.UTF8.GetBytes(poruka);

            try
            {

                int brBajta = garazaUDPSoket.SendTo(binarnaPoruka, 0, binarnaPoruka.Length, SocketFlags.None, garazaUDPPoint);

            }
            catch (SocketException ex)
            {
                Console.WriteLine($"Doslo je do greske tokom slanja poruke: \n{ex}");
            }

            //SLANJE PORUKE O STAZI AUTOMOBILU

            BinaryFormatter binaryFormatter = new BinaryFormatter();

            using (MemoryStream memoryStream = new MemoryStream())
            {
                binaryFormatter.Serialize(memoryStream, odabranaStaza);
                byte[] bytes = memoryStream.ToArray();
                garazaUDPSoket.SendTo(bytes, 0, bytes.Length, SocketFlags.None, garazaUDPPoint);

            }

            Socket automobilSocket = garazaTCPSoket.Accept();

            //SLANJE PORUKE O PREKIDU TRKE

            byte[] porukaPrekida = new byte[1024];

            Console.WriteLine("Da li zelite da se automobil vrati u garazu? (da/ne)");

            string odgovor = Console.ReadLine();

            if (odgovor == "da")
            {
                byte[] odgovorByte = Encoding.UTF8.GetBytes(odgovor);

                int brbajta = garazaUDPSoket.SendTo(odgovorByte, 0, odgovorByte.Length, SocketFlags.None, garazaUDPPoint);

            }
            else
            {

                byte[] odgovorByte = Encoding.UTF8.GetBytes(odgovor);

                int brbajta = garazaUDPSoket.SendTo(odgovorByte, 0, odgovorByte.Length, SocketFlags.None, garazaUDPPoint);

                Console.WriteLine("Automobil ce ostati na traci");
            }

            int prijemPoruka = automobilSocket.Receive(porukaPrekida);

            if (porukaPrekida.Length > 0)
            {

                string porukaGaraza = Encoding.UTF8.GetString(porukaPrekida);

                Console.WriteLine("\n"+porukaGaraza+"\n");

            }

            Console.WriteLine("Klijent zavrsava sa radom");
            Console.ReadLine();
            garazaUDPSoket.Close();
            garazaTCPSoket.Close();
            //acceptedSocket.Close();



        }
    }
}
