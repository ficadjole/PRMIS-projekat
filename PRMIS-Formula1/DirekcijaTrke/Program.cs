using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace DirekcijaTrke
{
    internal class Program
    {
        static void Main(string[] args)
        {

            Socket serverSocket = new Socket(AddressFamily.InterNetwork,SocketType.Stream,ProtocolType.Tcp);

            IPEndPoint localEndPoint = new IPEndPoint(IPAddress.Any,53000);
            serverSocket.Bind(localEndPoint);

            serverSocket.Listen(1000);

            Socket acceptedSocket = serverSocket.Accept();

            byte[] buffer = new byte[1024];
            int bytesRead = acceptedSocket.Receive(buffer);

            Console.WriteLine($"Poruka koju smo dobili je {Encoding.UTF8.GetString(buffer,0,bytesRead)}");
            Console.ReadKey();

        }
    }
}
