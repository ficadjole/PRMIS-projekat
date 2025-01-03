using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Garaza
{
    internal class Garaza
    {
        static void Main(string[] args)
        {

            Socket garazaSoket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            Console.WriteLine($"Podaci TCP uticnice: {garazaSoket.AddressFamily}:{garazaSoket.SocketType}:{garazaSoket.ProtocolType}");

            IPEndPoint garazaEndPoint = new IPEndPoint(IPAddress.Any, 53001);

            garazaSoket.Bind(garazaEndPoint);

            garazaSoket.Listen(1000);

            Console.WriteLine($"Pokrenut je server na {garazaEndPoint.Address}:{garazaEndPoint.Port}");

            Console.Write($"Unesite duzinu staze u kilometrima: ");
            int duzinaStaze = Int32.Parse(Console.ReadLine());
            Console.Write($"Unesite osnovno vreme kruga u sekundama: ");
            int vremeKruga = Int32.Parse(Console.ReadLine());

            Socket acceptedSocket = garazaSoket.Accept();

            Console.ReadKey();

            garazaSoket.Close();
            acceptedSocket.Close();

            

        }
    }
}
