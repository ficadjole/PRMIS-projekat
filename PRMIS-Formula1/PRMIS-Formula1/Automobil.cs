using Klase;
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
    internal class Automobil
    {
        static void Main(string[] args)
        {

            Console.WriteLine("Odaberite proizvodjaca automobila: ");
            Console.WriteLine("1. Mercedes");
            Console.WriteLine("2. Ferari");
            Console.WriteLine("3. Reno");
            Console.WriteLine("4. Honda");

            int unos;

            while(true)
            {

                unos = Int32.Parse(Console.ReadLine());

                if(unos > 0 && unos <= 4)
                {
                    break;
                   
                }
                else
                {
                    Console.WriteLine("Molim Vas unesite jedan od vazecih brojeva");
                }
            }

            AutomobiliRepository repo = new AutomobiliRepository();

            var odabranAuto = repo.OdabranAuto(unos);

            int odabraneGume;

            Console.WriteLine("Izaberite gume: ");
            Console.WriteLine("1. Meke-80km(M)");
            Console.WriteLine("2. Srednjetvrde-100km(S)");
            Console.WriteLine("3. Tvrde-120km(T)");

            while (true)
            {
                odabraneGume = Int32.Parse(Console.ReadLine());

                if(odabraneGume > 0 && odabraneGume <= 3)
                {
                    break;
                    
                }
                else
                {
                    Console.WriteLine("Molim Vas unesite jedan od vazecih brojeva");
                }
            }

            GumeRepository gumeRepository = new GumeRepository();

            var GumeOdabrane = gumeRepository.odabraneGume(odabraneGume);

            Console.WriteLine(odabranAuto.ToString());
            
            Console.WriteLine(GumeOdabrane.ToString());

            //Uspostavljanje TCP konekcije sa garazaom i otvaranje UDP uticnice

            Socket automobilTCPSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);

            Socket automobilUDPSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

            Console.WriteLine($"Podaci UDP uticnice: {automobilUDPSocket.AddressFamily}:{automobilUDPSocket.SocketType}:{automobilUDPSocket.ProtocolType}");

            IPEndPoint serverEndPoint = new IPEndPoint(IPAddress.Parse("192.168.0.33"), 53001);

            automobilTCPSocket.Connect(serverEndPoint);

            Console.WriteLine($"Povezani smo sa Garazom preko {serverEndPoint.Address}:{serverEndPoint.Port}");

            Console.WriteLine("Klijent zavsrava sa readom pritisnite enter");
            Console.ReadKey();
            automobilTCPSocket.Close();
            automobilUDPSocket.Close();

            //BinaryFormatter formatter = new BinaryFormatter();

            //while (true)
            //{
            //    Console.WriteLine("Unesite broj auta: ");
            //    string auto = Console.ReadLine();

            //    Console.WriteLine("Unesite koliko je bilo krugova: ");
            //    int krugovi = Int32.Parse(Console.ReadLine());
            //    List<double> vremena = new List<double>();

            //    for(int i = 0; i < krugovi; i++)
            //    {
            //        Console.WriteLine($"Unesite vreme za {i + 1}. krug: ");
            //        double vreme = Double.Parse(Console.ReadLine());

            //        vremena.Add(vreme);

            //    }

            //    Rezultati rezultati = new Rezultati();

            //    rezultati.Kljuc = auto;
            //    rezultati.Vremena = vremena;

            //    using (MemoryStream  ms = new MemoryStream())
            //    {
            //        formatter.Serialize(ms, rezultati);

            //        byte[] data = ms.ToArray();

            //        automobilSocket.Send(data);
            //    }

            //    Console.WriteLine("Uspesno ste poslali podatke! Da li zelite jos? da/ne");

            //    if (Console.ReadLine() != "da")
            //        break;

            //}



        }
    }
}
