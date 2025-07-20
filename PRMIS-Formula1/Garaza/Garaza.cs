using Garaza.Presentation;
using Garaza.Services;
using Klase;
using Klase.Models.Staze;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;

namespace Garaza
{
    internal class Garaza
    {
        static void Main(string[] args)
        {

            Console.WriteLine("--------------- GARAZA ---------------");


            //KREIRANJE TCP UTICNICE I TCP SERVERA - preko toga se salju podaci od automobila ka garazi

            Socket garazaTCPSoket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            Console.WriteLine($"Podaci TCP uticnice: {garazaTCPSoket.AddressFamily}:{garazaTCPSoket.SocketType}:{garazaTCPSoket.ProtocolType}");


            EndPoint garazaTCPPoint = new IPEndPoint(IPAddress.Parse("192.168.0.32"), 53003);

            garazaTCPSoket.Bind(garazaTCPPoint);

            garazaTCPSoket.Listen(20);

            //KREIRANJE UDP UTICNICE - preko nje garaza salje podatke automobilu

            Socket garazaUDPSoket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

            EndPoint garazaUDPPoint = new IPEndPoint(IPAddress.Parse("192.168.0.32"), 53005);

            //OVO OVDE JE DEO ZA VISE KORISNIKA DA SE ZNA KOME SALJE PODATKE
            EndPoint posiljaocEP = new IPEndPoint(IPAddress.Any, 53006);

            garazaUDPSoket.Bind(posiljaocEP);

            int n = 0, brB = 0;
            byte[] bytes1 = new byte[1024];
            BinaryFormatter binaryFormatter1 = new BinaryFormatter();
            EndPoint automobilovEndPoint = null;
            List<EndPoint> automobilEndPoints = new List<EndPoint>();
            do
            {

                if (garazaUDPSoket.Poll(1000 * 1000, SelectMode.SelectRead))
                {

                    brB = garazaUDPSoket.ReceiveFrom(bytes1, ref posiljaocEP); //slanje poruke da se otvori UDP uticnica

                    if (brB > 0)
                    {

                        using (MemoryStream ms = new MemoryStream(bytes1, 0, brB))
                        {
                            automobilovEndPoint = (EndPoint)binaryFormatter1.Deserialize(ms);
                            automobilEndPoints.Add(automobilovEndPoint);
                        }

                        Console.WriteLine($"PRIJAVLJEN AUTO:{automobilovEndPoint.ToString()}");
                    }

                }


                n++;
            } while (n != 20);



            //ODABIR STAZE, GUMA i GORIVA

            Staza odabranaStaza = new OdabirStaze().odabirStaze();

            Console.WriteLine(odabranaStaza.ToString());

            Gume odabraneGume = new OdabirGuma().odabraneGume();

            Console.WriteLine("Unesite kolicinu goriva koja se nalazi u kolima: ");

            int kolicinaGoriva = Int32.Parse(Console.ReadLine());

            Console.WriteLine(odabraneGume.ToString());

            //PARSIRANJE PORUKE ZA SLANJE AUTOMOBILU

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

            //SLANJE PORUKE AUTOMOBILU O GUMAMA I GORIVU

            byte[] binarnaPoruka = Encoding.UTF8.GetBytes(poruka);
            Socket nekiSoket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

            try
            {

                foreach (EndPoint EP in automobilEndPoints)
                {
                    garazaUDPSoket.SendTo(binarnaPoruka, 0, binarnaPoruka.Length, SocketFlags.None, EP);
                }

            }
            catch (SocketException ex)
            {
                Console.WriteLine($"Doslo je do greske tokom slanja poruke: \n{ex}");
            }

            //SLANJE PORUKE O STAZI AUTOMOBILU

            BinaryFormatter binaryFormatter = new BinaryFormatter();

            using (MemoryStream memoryStream = new MemoryStream())
            {
                binaryFormatter.Serialize(memoryStream, odabranaStaza);
                byte[] bytes = memoryStream.ToArray();
                foreach (EndPoint EP in automobilEndPoints)
                {
                    garazaUDPSoket.SendTo(bytes, 0, bytes.Length, SocketFlags.None, EP);
                }

            }

            foreach (EndPoint EP in automobilEndPoints)
            {
                automobilovEndPoint = EP; //postavljamo automobilovEndPoint na poslednji primljeni EndPoint

                Console.WriteLine($"\n-------------------Prikacio se automobil {automobilovEndPoint.ToString()}-------------------");


                Socket automobilSocket = garazaTCPSoket.Accept();


                //SLANJE PORUKE O PREKIDU TRKE

                automobilSocket.Blocking = false; //postavljanje neblokirajuceg moda za socket

                bool prekid = false;

                Thread slanjeDirektive = new Thread(() =>
                {

                    new SlanjeDirektiva().slanjeDirektiva(garazaUDPSoket, ref automobilovEndPoint, ref prekid);

                });

                slanjeDirektive.IsBackground = true;

                slanjeDirektive.Start();

                //PRIMANJE PORUKE OD AUTOMOBILA O PREKIDU TRKE ILI O GUMAMA I GORIVU

                byte[] porukaPrekida = new byte[1024];

                int brKrugova = 0;

                while (brKrugova != 11)
                {

                    List<Socket> readSockets = new List<Socket> { automobilSocket };

                    Socket.Select(readSockets, null, null, 1000); //čekanje na podatke sa socket-a sa timeout-om od 1 sekunde

                    if (readSockets.Count > 0)
                    {
                        int prijemPoruka = automobilSocket.Receive(porukaPrekida);

                        if (prekid == true)
                        {
                            slanjeDirektive.Abort(); //prekidamo trenutnu nit nakon slanja poruke da se automobil vrati u garazu
                            break;
                        }

                        if (prijemPoruka > 0)
                        {
                            string porukaGaraza = Encoding.UTF8.GetString(porukaPrekida);

                            if (porukaGaraza == "\n>>AUTOMOBIL: vrednosti guma ili goriva manje od bezbednih povratak u garazu!!\n" || porukaGaraza == "\n>>AUTOMOBIL: vracam se u garazu!!\n")
                            {
                                Console.WriteLine(porukaGaraza);
                                break;
                            }
                            else
                            {
                                Console.WriteLine(porukaGaraza);
                            }
                        }


                        brKrugova++;
                    }

                }
                automobilSocket.Close();
            }

            Console.WriteLine("Klijent zavrsava sa radom");
            Console.ReadLine();
            garazaUDPSoket.Close();
            garazaTCPSoket.Close();

        }

    }
}

