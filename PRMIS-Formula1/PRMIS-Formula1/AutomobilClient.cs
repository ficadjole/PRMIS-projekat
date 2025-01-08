using Klase;
using PRMIS_Formula1.Presentation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace PRMIS_Formula1
{
    internal class AutomobilClient
    {
        static void Main(string[] args)
        {
            Console.WriteLine("--------------- Automobil ---------------");

            var automobil = new OdabirKonfiguracije().odabirKonfiugracijeIGuma();

            Console.WriteLine(automobil.ToString());    

            //Uspostavljanje TCP konekcije sa garazaom i otvaranje UDP uticnice

            Socket automobilTCPSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);

            Socket automobilUDPSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

            Console.WriteLine($"Podaci UDP uticnice: {automobilUDPSocket.AddressFamily}:{automobilUDPSocket.SocketType}:{automobilUDPSocket.ProtocolType}");

            EndPoint serverEndPoint = new IPEndPoint(IPAddress.Parse("192.168.0.28"), 53002); //promeniti ip adresu u zavisnoti na kom racunaru se radi

            automobilUDPSocket.Bind(serverEndPoint);

            //automobilTCPSocket.Connect(serverEndPoint);

            BinaryFormatter binaryFormatter = new BinaryFormatter();

            byte[] buffer = new byte[1024];



            while (true)
            {
                int brBajta = automobilUDPSocket.ReceiveFrom(buffer, ref serverEndPoint);

                if (brBajta == 0)
                {
                    break;
                }

                using (MemoryStream ms = new MemoryStream(buffer, 0, brBajta))
                {
                    Gume gume = (Gume)binaryFormatter.Deserialize(ms);

                    automobil.gumeAutomobila = gume;
                }

                Console.WriteLine(automobil.ToString());

            }

            Console.WriteLine("Server zavrsava sa radom");




            Console.WriteLine("Klijent zavsrava sa readom pritisnite enter");
            Console.ReadKey();

            automobilTCPSocket.Close();
            automobilUDPSocket.Close();

            



        }
    }
}
