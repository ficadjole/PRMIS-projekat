
using DirekcijaTrke.PomocneFunkcije;
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

            //Recnik za vodjenje evidencije o vremenu
            Dictionary<string, List<double>> podaciOAutima = new Dictionary<string, List<double>>(); //(trkacki broj+proizovdjac,vremena po krugu)
            List<Automobil> listaAutomobila = new List<Automobil>();


            //KREIRANJE TCP SOKETA I SERVERA ZA DIREKCIJU TRKE
            Socket serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            
            Console.WriteLine($"Otvorena je TCP uticnica {serverSocket.AddressFamily}:{serverSocket.SocketType}:{serverSocket.ProtocolType}");

            IPEndPoint localEndPoint = new IPEndPoint(IPAddress.Any, 53001);
            serverSocket.Bind(localEndPoint);

            serverSocket.Listen(1000);

            Socket acceptedSocket = serverSocket.Accept();

            //PRIMANJE PORUKE OD AUTOMOBILA O ODABRANOM AUTOMOBILU

            BinaryFormatter binaryFormatter = new BinaryFormatter();

            byte[] buffer = new byte[1024];

            int bytesRead = acceptedSocket.Receive(buffer);

            using (MemoryStream ms = new MemoryStream(buffer, 0, bytesRead))
            {
                Automobil automobil = (Automobil)binaryFormatter.Deserialize(ms);

                listaAutomobila.Add(automobil);

            }


            //SLANJE STARTNOG BROJA PRIJAVLJENOM AUTOMOBILU

            acceptedSocket.Send(Encoding.UTF8.GetBytes($"{listaAutomobila.Count}"));

            //PRIMANJE VREMENA

            byte[] buffer2 = new byte[1024];
            int brKrugova = 0;
            int brBajta;
            string vremeIBroj;
            string kljucRecnika = "";
            double vremePoKrugu;
            List<double> vremeKrugova = new List<double>();

            while(brKrugova!=10)
            {
                brBajta = acceptedSocket.Receive(buffer2);

                if (brBajta == 0)
                    break;

                vremeIBroj = Encoding.UTF8.GetString(buffer2);

                string[] delovi = vremeIBroj.Split('/');

                vremePoKrugu = Double.Parse(delovi[0]);
                kljucRecnika = delovi[1];

                Console.WriteLine($"Vreme kruga je: {vremePoKrugu} / {kljucRecnika}");

                vremeKrugova.Add(vremePoKrugu);

                brKrugova++;

            }

            //DODAVANJE AUTOMOBILA I VREMENA U RECNIK 

            if (podaciOAutima.ContainsKey(kljucRecnika) == false)
            {
                podaciOAutima.Add(kljucRecnika,vremeKrugova);
            }


            double najboljeVreme = new NajbrziKrug().ProveraNajbrzegKruga(vremeKrugova);

            Console.WriteLine($"Najbrzi krug je: {najboljeVreme}");

            Console.WriteLine("Server zavrsava sa radom");

            Console.ReadLine();
            acceptedSocket.Close();



        }
    }
}
