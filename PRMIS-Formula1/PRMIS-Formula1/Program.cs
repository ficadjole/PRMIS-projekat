using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace PRMIS_Formula1
{
    internal class Program
    {
        static void Main(string[] args)
        {

            Socket automobilSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);

            IPEndPoint serverEndPoint = new IPEndPoint(IPAddress.Parse("192.168.0.41"),53000);

            automobilSocket.Connect(serverEndPoint);

            Console.WriteLine("Unesite zeljeni tekst: ");

            string buffer = Console.ReadLine();

            int byteSent = automobilSocket.Send(Encoding.UTF8.GetBytes(buffer));
            Console.ReadKey();
        }
    }
}
