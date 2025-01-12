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

            Socket garazaTCPSoket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            Socket garazaUDPSoket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

            Console.WriteLine($"Podaci TCP uticnice: {garazaTCPSoket.AddressFamily}:{garazaTCPSoket.SocketType}:{garazaTCPSoket.ProtocolType}");

            EndPoint garazaEndPoint = new IPEndPoint(IPAddress.Any, 53001);

            EndPoint garazaUDPPoint = new IPEndPoint(IPAddress.Parse("192.168.0.28"), 53002);

            Staza odabranaStaza = new OdabirStaze().odabirStaze();

            Console.WriteLine(odabranaStaza.ToString());

            Gume odabraneGume = new OdabirGuma().odabraneGume();

            Console.WriteLine("Unesite kolicinu goriva koja se nalazi u kolima: ");

            int kolicinaGoriva = Int32.Parse(Console.ReadLine());

            Console.WriteLine(odabraneGume.ToString());

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

            byte[] binarnaPoruka = Encoding.UTF8.GetBytes(poruka);

            try
            {

                int brBajta = garazaUDPSoket.SendTo(binarnaPoruka, 0, binarnaPoruka.Length, SocketFlags.None, garazaUDPPoint);

            }
            catch (SocketException ex)
            {
                Console.WriteLine($"Doslo je do greske tokom slanja poruke: \n{ex}");
            }

            BinaryFormatter binaryFormatter = new BinaryFormatter();

            using (MemoryStream memoryStream = new MemoryStream())
            {
                binaryFormatter.Serialize(memoryStream, odabranaStaza);
                byte[] bytes = memoryStream.ToArray();
                garazaUDPSoket.SendTo(bytes, 0, bytes.Length, SocketFlags.None, garazaUDPPoint);

            }



            Console.WriteLine("Klijent zavrsava sa radom");
            Console.ReadLine();
            garazaUDPSoket.Close();
            garazaTCPSoket.Close();
            //acceptedSocket.Close();



        }
    }
}
