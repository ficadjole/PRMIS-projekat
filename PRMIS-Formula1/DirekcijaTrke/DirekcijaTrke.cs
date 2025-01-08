
using PRMIS_Formula1.Models.Automobil;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace DirekcijaTrke
{
    internal class DirekcijaTrke
    {
        static void Main(string[] args)
        {
            Console.WriteLine("--------------- Direkcija Trke ---------------");

            Dictionary<string, List<double>> podaciOAutima = new Dictionary<string, List<double>>(); //(trkacki broj+proizovdjac,vremena po krugu)
            List<Automobil> listaAutomobila = new List<Automobil>();
            Socket serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            Console.WriteLine($"Otvorena je TCP uticnica {serverSocket.AddressFamily}:{serverSocket.SocketType}:{serverSocket.ProtocolType}");

            IPEndPoint localEndPoint = new IPEndPoint(IPAddress.Any, 53001);
            serverSocket.Bind(localEndPoint);

            serverSocket.Listen(1000);

            Socket acceptedSocket = serverSocket.Accept();

            BinaryFormatter binaryFormatter = new BinaryFormatter();

            byte[] buffer = new byte[1024];

            int bytesRead = acceptedSocket.Receive(buffer);

            using (MemoryStream ms = new MemoryStream(buffer, 0, bytesRead))
            {
                Automobil automobil = (Automobil)binaryFormatter.Deserialize(ms);

                podaciOAutima.Add((podaciOAutima.Count + 1) + " " + automobil.konfiguracijaAutomobila.ime, new List<double>());
                listaAutomobila.Add(automobil);
            }

            acceptedSocket.Send(Encoding.UTF8.GetBytes($"{podaciOAutima.Count}"));

            Console.WriteLine("Server zavrsava sa radom");

            Console.ReadLine();
            acceptedSocket.Close();



        }
    }
}
