using Klase.Models.Staze;
using PRMIS_Formula1.Models.Automobil;
using PRMIS_Formula1.Presentation;
using PRMIS_Formula1.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace PRMIS_Formula1
{
    internal class AutomobilClient
    {
        static void Main(string[] args)
        {
            Console.WriteLine("--------------- Automobil ---------------");

            //ODABIR KONFIGURACIJE AUTOMOBILA

            Automobil automobil = new OdabirKonfiguracije().odabirKonfiugracijeIGuma();

            Console.WriteLine(automobil.ToString());

            //Uspostavljanje TCP konekcije sa garazaom i direkcijom trke

            Socket automobilTCPSocketDirekcija = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            Socket automobilTCPSocketGaraza = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            EndPoint serverEndPointTCPDirekcija = new IPEndPoint(IPAddress.Parse("192.168.0.34"), 53001);

            EndPoint serverEndPointTCPGaraza = new IPEndPoint(IPAddress.Parse("192.168.0.34"), 53003);

            //OTVARANJE UDP UTICNICE I SERVERA ZA KOMUNIKACIJU SA GARAZOM

            Socket automobilUDPSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

            Console.WriteLine($"Podaci UDP uticnice: {automobilUDPSocket.AddressFamily}:{automobilUDPSocket.SocketType}:{automobilUDPSocket.ProtocolType}");

            EndPoint serverEndPointUDP = new IPEndPoint(IPAddress.Parse("192.168.0.34"), 53002); //promeniti ip adresu u zavisnoti na kom racunaru se radi

            automobilUDPSocket.Bind(serverEndPointUDP);

            //PRIMANJE PODATAKA O GORIVU I GUMAMA OD GARAZE

            byte[] buffer = new byte[1024];

            int brBajta = automobilUDPSocket.ReceiveFrom(buffer, ref serverEndPointUDP); //ovde dobijamo podatke o gorivu i gumama

            string poruka = Encoding.UTF8.GetString(buffer, 0, brBajta);

            Console.WriteLine(poruka);

            new ParsiranjePorukeGaraze().parsiranjePorukeGaraze(poruka, ref automobil); //parsiranje dobijene poruke

            //PRIMANJE PODATAKA O STAZI

            BinaryFormatter binaryFormatter = new BinaryFormatter();

            int brBajtaStaza = automobilUDPSocket.ReceiveFrom(buffer, ref serverEndPointUDP); //ovde dobijamo podatke o stazi

            Staza staza = new Staza();

            using (MemoryStream ms = new MemoryStream(buffer, 0, brBajtaStaza))
            {
                staza = (Staza)binaryFormatter.Deserialize(ms);
            }

            //PRIKLJUCIVANJE NA DIREKCIJU TRKE

            automobilTCPSocketDirekcija.Connect(serverEndPointTCPDirekcija); //ovde se prikljucujemo na direkcijuTrke

            //SLANJE DIREKCIJI TRKE PODATKE O AUTOMOBILU

            using (MemoryStream ms = new MemoryStream())
            {
                binaryFormatter.Serialize(ms, automobil);
                byte[] bytes = ms.ToArray();
                automobilTCPSocketDirekcija.Send(bytes);
            }

            //PRIMANJE TRKACKOG BROJA OD DIREKCIJE TRKE

            int br = automobilTCPSocketDirekcija.Receive(buffer);

            automobil.trkackiBroj = Int32.Parse(Encoding.UTF8.GetString(buffer, 0, br));

            Console.WriteLine(automobil);


            //SIMULACIJA VOZNJE TRKE I KONEKTOVANJE NA TCP SERVER GARAZE

            automobilTCPSocketGaraza.Connect(serverEndPointTCPGaraza);


            List<double> vremenaPoKrugu = new SimulacijaTrke().simulacija(ref automobil, staza, automobilTCPSocketGaraza, automobilUDPSocket, automobilTCPSocketDirekcija , ref serverEndPointUDP);

            Console.WriteLine();

            Console.WriteLine();
            Console.WriteLine(automobil);

            Console.WriteLine("\nKlijent zavsrava sa radom pritisnite enter");
            Console.ReadKey();

            automobilTCPSocketDirekcija.Close();
            automobilUDPSocket.Close();
            automobilTCPSocketGaraza.Close();




        }
    }
}
