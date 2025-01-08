using PRMIS_Formula1.Models.Automobil;
using PRMIS_Formula1.Presentation;
using System;
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

            Automobil automobil = new OdabirKonfiguracije().odabirKonfiugracijeIGuma();

            Console.WriteLine(automobil.ToString());

            //Uspostavljanje TCP konekcije sa garazaom i otvaranje UDP uticnice

            Socket automobilTCPSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);

            Socket automobilUDPSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

            Console.WriteLine($"Podaci UDP uticnice: {automobilUDPSocket.AddressFamily}:{automobilUDPSocket.SocketType}:{automobilUDPSocket.ProtocolType}");

            EndPoint serverEndPointUDP = new IPEndPoint(IPAddress.Parse("192.168.0.28"), 53002); //promeniti ip adresu u zavisnoti na kom racunaru se radi

            EndPoint serverEndPointTCP = new IPEndPoint(IPAddress.Parse("192.168.0.28"), 53001);

            automobilUDPSocket.Bind(serverEndPointUDP);

            byte[] buffer = new byte[1024];

            int brBajta = automobilUDPSocket.ReceiveFrom(buffer, ref serverEndPointUDP);

            string poruka = Encoding.UTF8.GetString(buffer, 0, brBajta);

            Console.WriteLine(poruka);

            new ParsiranjePorukeGaraze().parsiranjePorukeGaraze(poruka, ref automobil);

            Console.WriteLine(automobil.ToString());

            automobilTCPSocket.Connect(serverEndPointTCP);

            BinaryFormatter binaryFormatter = new BinaryFormatter();

            using (MemoryStream ms = new MemoryStream())
            {
                binaryFormatter.Serialize(ms, automobil);
                byte[] bytes = ms.ToArray();
                automobilTCPSocket.Send(bytes);
            }

            int br = automobilTCPSocket.Receive(buffer);

            automobil.trkackiBroj = Int32.Parse(Encoding.UTF8.GetString(buffer, 0, br));

            Console.WriteLine(automobil);

            Console.WriteLine("Klijent zavsrava sa radom pritisnite enter");
            Console.ReadKey();

            automobilTCPSocket.Close();
            automobilUDPSocket.Close();





        }
    }
}
