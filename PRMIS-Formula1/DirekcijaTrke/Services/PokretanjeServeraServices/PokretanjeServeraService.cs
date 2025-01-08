using DirekcijaTrke.Servisi;
using System.Net.Sockets;
using System.Net;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using Klase;
using System.Collections.Generic;
using System;

namespace DirekcijaTrke.Services.PokretanjeServeraServices
{
    public class PokretanjeServeraService : IPokretanjeServera
    {
        public void PokretanjeServera()
        {

            Dictionary<string,List<double>> podaciOAutu = new Dictionary<string,List<double>>();

            Socket serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            Console.WriteLine($"Otvorena je TCP uticnica {serverSocket.AddressFamily}:{serverSocket.SocketType}:{serverSocket.ProtocolType}");

            IPEndPoint localEndPoint = new IPEndPoint(IPAddress.Any, 53000);
            serverSocket.Bind(localEndPoint);

            serverSocket.Listen(1000);

            Socket acceptedSocket = serverSocket.Accept();

            //BinaryFormatter binaryFormatter = new BinaryFormatter();

            //byte[] buffer = new byte[1024];

            

            //while(true)
            //{
            //    int brBajta = acceptedSocket.Receive(buffer);

            //    if(brBajta == 0)
            //    {
            //        break;
            //    }

            //    using(MemoryStream ms = new MemoryStream(buffer,0,brBajta))
            //    {
            //        Rezultati rezultati = (Rezultati)binaryFormatter.Deserialize(ms);

            //        podaciOAutu.Add(rezultati.Kljuc,rezultati.Vremena);

            //        Console.WriteLine($"Broj automobila koji je dodat je: {rezultati.Kljuc}");
            //        Console.Write($"Vremena dodatog automobila su: ");
            //        foreach(var vreme in rezultati.Vremena)
            //        {
            //            Console.Write($"{vreme} s ");
            //        }
            //        Console.WriteLine();
            //    }

            //}

            //Console.WriteLine("Server zavrsava sa radom");

            Console.ReadLine();
            acceptedSocket.Close();

        }
    }
}
