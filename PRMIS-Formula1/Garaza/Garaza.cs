using Garaza.Presentation;
using Klase;
using Klase.Models.Staze;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace Garaza
{
    internal class Garaza
    {
        static void Main(string[] args)
        {

            Console.WriteLine("--------------- GARAZA ---------------");

            Socket garazaTCPSoket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            Socket garazaUDPSoket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

            Console.WriteLine($"Podaci TCP uticnice: {garazaTCPSoket.AddressFamily}:{garazaTCPSoket.SocketType}:{garazaTCPSoket.ProtocolType}");

            EndPoint garazaEndPoint = new IPEndPoint(IPAddress.Any, 53001);

            EndPoint garazaUDPPoint = new IPEndPoint(IPAddress.Any, 53002);

            Staza odabranaStaza = new OdabirStaze().odabirStaze();

            Console.WriteLine(odabranaStaza.ToString());

            Gume odabraneGume = new OdabirGuma().odabraneGume();

            Console.WriteLine(odabraneGume.ToString());

            //garazaTCPSoket.Bind(garazaEndPoint);

            //garazaTCPSoket.Listen(1000);

            //Console.WriteLine($"Pokrenut je server na {garazaEndPoint.Address}:{garazaEndPoint.Port}");

            //Socket acceptedSocket = garazaTCPSoket.Accept();

            BinaryFormatter binaryFormatter = new BinaryFormatter();

            while(true)
            {
                byte[] buffer = new byte[1024];

                using (MemoryStream ms = new MemoryStream())
                {
                    binaryFormatter.Serialize(ms, odabraneGume);

                    buffer = ms.ToArray();

                    garazaUDPSoket.SendTo(buffer, 0, buffer.Length, SocketFlags.None, garazaUDPPoint);

                }

                Console.WriteLine("Uspesno ste poslali podatke! Da li zelite jos? da/ne");

                if (Console.ReadLine() != "da")
                    break;
            }

            garazaUDPSoket.Close();
            garazaTCPSoket.Close();
            //acceptedSocket.Close();

            

        }
    }
}
