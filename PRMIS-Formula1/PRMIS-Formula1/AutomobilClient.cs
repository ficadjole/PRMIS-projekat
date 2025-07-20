using Klase.Models.Staze;
using PRMIS_Formula1.Models.Automobil;
using PRMIS_Formula1.Presentation;
using PRMIS_Formula1.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.NetworkInformation;
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

            EndPoint serverEndPointTCPDirekcija = new IPEndPoint(IPAddress.Parse("192.168.0.32"), 53001);

            EndPoint serverEndPointTCPGaraza = new IPEndPoint(IPAddress.Parse("192.168.0.32"), 53003);

            //OTVARANJE UDP UTICNICE I SERVERA ZA KOMUNIKACIJU SA GARAZOM

            Socket automobilUDPSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

            Console.WriteLine($"Podaci UDP uticnice: {automobilUDPSocket.AddressFamily}:{automobilUDPSocket.SocketType}:{automobilUDPSocket.ProtocolType}");

            IPGlobalProperties iPGlobalProperties = IPGlobalProperties.GetIPGlobalProperties();

            IPEndPoint[] udpConnections = iPGlobalProperties.GetActiveUdpListeners();

            bool isPortInUse = false;

            foreach (IPEndPoint udpConnection in udpConnections)
            {

                if (udpConnection.Port == 53005)
                {
                    Console.WriteLine("UDP port 53005 is already in use. Please choose a different port.");
                    isPortInUse = true;
                }

            }

            EndPoint serverEndPointUDP;
            if (isPortInUse)
            {
                serverEndPointUDP = new IPEndPoint(IPAddress.Parse("192.168.0.32"), 53010); //promeniti ip adresu u zavisnoti na kom racunaru se radi
            }
            else
            {
                serverEndPointUDP = new IPEndPoint(IPAddress.Parse("192.168.0.32"), 53005);
            }


            automobilUDPSocket.Bind(serverEndPointUDP);


            EndPoint posiljaocEP = new IPEndPoint(IPAddress.Parse("192.168.0.32"), 53006); //ovde cemo slati podatke 

            //SLANJE PODATAKA O PRIJAVLJENOM AUTOMOBILU GARAZI
            BinaryFormatter binaryFormatterPocetak = new BinaryFormatter();

            //slanje podataka o automobilu garazi
            using (MemoryStream ms = new MemoryStream())
            {

                binaryFormatterPocetak.Serialize(ms, serverEndPointUDP);
                byte[] bytes = ms.ToArray();
                automobilUDPSocket.SendTo(bytes, 0, bytes.Length, SocketFlags.None, posiljaocEP);

            }

            Console.WriteLine("Poruka je poslata");

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

            automobilTCPSocketDirekcija.Blocking = false;

            //SLANJE DIREKCIJI TRKE PODATKE O AUTOMOBILU

            while (true)
            {
                List<Socket> writeSockets = new List<Socket> { automobilTCPSocketDirekcija };

                Socket.Select(null, writeSockets, null, 1000); // cekamo da li je socket spreman za slanje podataka  

                if (writeSockets.Count > 0)
                {
                    using (MemoryStream ms = new MemoryStream())
                    {
                        binaryFormatter.Serialize(ms, automobil);
                        byte[] bytes = ms.ToArray();
                        automobilTCPSocketDirekcija.Send(bytes);
                        writeSockets.Clear(); //ocistimo listu nakon slanja podataka
                    }
                }

                List<Socket> readSockets = new List<Socket> { automobilTCPSocketDirekcija };

                Socket.Select(readSockets, null, null, 1000); // cekamo da li je socket spreman za primanje podataka    

                if (readSockets.Count > 0)
                {
                    //PRIMANJE TRKACKOG BROJA OD DIREKCIJE TRKE

                    int br = automobilTCPSocketDirekcija.Receive(buffer);

                    automobil.trkackiBroj = Int32.Parse(Encoding.UTF8.GetString(buffer, 0, br));

                    Console.WriteLine(automobil);
                }

                if (automobil.trkackiBroj != 0)
                {
                    break; //izlazimo iz petlje kada dobijemo trkacki broj
                }




            }

            //SIMULACIJA VOZNJE TRKE I KONEKTOVANJE NA TCP SERVER GARAZE

            automobilTCPSocketGaraza.Connect(serverEndPointTCPGaraza);


            List<double> vremenaPoKrugu = new SimulacijaTrke().simulacija(ref automobil, staza, automobilTCPSocketGaraza, automobilUDPSocket, automobilTCPSocketDirekcija, ref posiljaocEP);

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
