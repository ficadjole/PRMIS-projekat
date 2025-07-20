
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
        Dictionary<string, double> najbrzaVremena = new Dictionary<string, double>();//najbrza vremena svakog automobila
        public bool dodajNajbrzeVreme(double vreme, Automobil auto)
        {
            najbrzaVremena.Add(auto.trkackiBroj + auto.konfiguracijaAutomobila.ime, vreme);
            return najbrzaVremena.Count > 0 ? true : false;
        }
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
            serverSocket.Blocking = false; //Postavljanje neblokirajuceg moda
            serverSocket.Listen(2000);


            List<Socket> listaKlijenataAutomobila = new List<Socket>();

            byte[] buffer = new byte[1024];


            bool v = false;
            while (true)
            {

                List<Socket> readSocketsAutomobili = new List<Socket>();

                if (listaKlijenataAutomobila.Count < 2000)
                {
                    readSocketsAutomobili.Add(serverSocket);
                }

                foreach (Socket s in listaKlijenataAutomobila)
                {
                    readSocketsAutomobili.Add(s);
                }

                Socket.Select(readSocketsAutomobili, null, null, 1000);

                if (readSocketsAutomobili.Count > 0)
                {


                    Console.WriteLine($"Broj dogadjaja je {readSocketsAutomobili.Count}");

                    foreach (Socket s in readSocketsAutomobili)
                    {
                        if (s == serverSocket)
                        {
                            //PRIMANJE PODATAKA OD AUTOMOBILA
                            Socket acceptedSocket = serverSocket.Accept();
                            acceptedSocket.Blocking = false; //Postavljanje neblokirajuceg moda
                            listaKlijenataAutomobila.Add(acceptedSocket);
                            Console.WriteLine($"Klijent se povezao sa {acceptedSocket.RemoteEndPoint}");
                        }
                        else
                        {

                            int brBajta2 = s.Receive(buffer);

                            if (brBajta2 == 0)
                            {
                                Console.WriteLine("Klijent je prekinuo komunikaciju");
                                s.Close();
                                listaKlijenataAutomobila.Remove(s);

                                if (listaKlijenataAutomobila.Count == 0)
                                {
                                    Console.WriteLine("Nema vise klijenata, server se gasi");

                                    Console.WriteLine("Server zavrsava sa radom");
                                    Console.ReadLine();
                                    return;
                                }

                                continue;
                            }
                            else
                            {

                                using (MemoryStream ms = new MemoryStream(buffer, 0, brBajta2))
                                {
                                    BinaryFormatter binaryFormatter = new BinaryFormatter();
                                    Automobil automobil = (Automobil)binaryFormatter.Deserialize(ms);
                                    listaAutomobila.Add(automobil);
                                    Console.WriteLine($"Automobil: {automobil.konfiguracijaAutomobila.ime} je dodat u listu");

                                }

                                s.Send(Encoding.UTF8.GetBytes($"{listaAutomobila.Count}"));
                                //PRIMANJE VREMENA

                                byte[] buffer2 = new byte[1024];
                                int brKrugova = 0;
                                int brBajta;
                                string vremeIBroj;
                                string kljucRecnika = "";
                                double vremePoKrugu;
                                List<double> vremeKrugova = new List<double>();

                                while (brKrugova != 11)
                                {
                                    List<Socket> readSockets = new List<Socket>() { s };

                                    Socket.Select(readSockets, null, null, 1000);

                                    if (readSockets.Count > 0)
                                    {

                                        brBajta = s.Receive(buffer2);

                                    }
                                    else
                                    {
                                        continue;
                                    }



                                    if (brBajta == 0)
                                        break;

                                    if (Encoding.UTF8.GetString(buffer2, 0, brBajta) == "\n>>AUTOMOBIL: vrednosti guma ili goriva manje od bezbednih povratak u garazu!!\n" || Encoding.UTF8.GetString(buffer2, 0, brBajta) == "\n>>AUTOMOBIL: vracam se u garazu!!\n")
                                    {
                                        Console.WriteLine(Encoding.UTF8.GetString(buffer2, 0, brBajta));
                                        break;
                                    }

                                    vremeIBroj = Encoding.UTF8.GetString(buffer2, 0, brBajta);

                                    if (vremeIBroj == "") continue;

                                    if (vremeIBroj.Contains("/"))
                                    {
                                        string[] delovi = vremeIBroj.Split('/');

                                        vremePoKrugu = Double.Parse(delovi[0]);
                                        kljucRecnika = delovi[1];

                                        //Console.WriteLine($"Vreme kruga je: {vremePoKrugu} / {kljucRecnika}");

                                        vremeKrugova.Add(vremePoKrugu);

                                        vremeKrugova.Sort();

                                        vremeKrugova.Reverse();

                                        Console.WriteLine();
                                        Console.Write($">>Vremena za {kljucRecnika}: ");
                                        for (int i = 0; i < vremeKrugova.Count; i++)
                                        {
                                            Console.Write($"{Math.Round(vremeKrugova[i], 2)} ");
                                        }

                                        Console.WriteLine();

                                        brKrugova++;
                                    }

                                }

                                //DODAVANJE AUTOMOBILA I VREMENA U RECNIK 

                                if (podaciOAutima.ContainsKey(kljucRecnika) == false)
                                {
                                    podaciOAutima.Add(kljucRecnika, vremeKrugova);
                                }

                                double najboljeVreme = new NajbrziKrug().ProveraNajbrzegKruga(vremeKrugova);

                                v = new DirekcijaTrke().dodajNajbrzeVreme(najboljeVreme, listaAutomobila[listaAutomobila.Count - 1]);

                                if (v)
                                {   //Ispisivanje rezultata trke
                                    Console.WriteLine("\n>>REZULTATI TRKE<<\n");
                                    foreach (KeyValuePair<string, List<double>> kvp in podaciOAutima)
                                    {
                                        Console.Write($"Automobil: {kvp.Key} - Vreme za najbrzi krug: ");
                                        Console.WriteLine(Math.Round(kvp.Value[kvp.Value.Count - 1], 2));
                                        Console.WriteLine();

                                    }
                                }
                                else
                                {
                                    Console.WriteLine("Nije uspelo dodavanje najbrzeg vremena");
                                }
                                break;

                            }


                        }
                    }

                }

                readSocketsAutomobili.Clear();

            }

        }

    }
}
