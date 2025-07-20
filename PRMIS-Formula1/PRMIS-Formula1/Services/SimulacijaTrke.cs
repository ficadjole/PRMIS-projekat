using Klase.Models.Staze;
using PRMIS_Formula1.Models.Automobil;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace PRMIS_Formula1.Services
{
    public class SimulacijaTrke
    {

        public List<double> simulacija(ref Automobil automobil, Staza staza, Socket automobilTCPSocket, Socket automobilUDPSocket, Socket automobilTCPSocketDirekcija, ref EndPoint serverEndPointUDP)
        {


            int n = 0;
            int pocetnaVrednostGuma = automobil.gumeAutomobila.duzinaKoriscenja;
            int brBajta;
            List<double> vremenaPoKrugu = new List<double>();


            string porukeGraze = "0";
            double vremeKruga = 0.0;
            int brojacSporeVoznje = 0;
            do
            {

                //PROVERA DA LI JE IZ GARAZE STIGLA PORUKA O PREKIDU TRKE
                byte[] kraj = new byte[1024];

                if (automobilUDPSocket.Poll(1000 * 1000, SelectMode.SelectRead))
                {
                    brBajta = automobilUDPSocket.ReceiveFrom(kraj, ref serverEndPointUDP);

                    if (brBajta > 0)
                    {
                        porukeGraze = Encoding.UTF8.GetString(kraj, 0, brBajta);

                        if (porukeGraze == "da")
                        {
                            Console.WriteLine("\n>>GARAZA: vrati se u garazu!!\n");
                            byte[] porukaDirekciji = Encoding.UTF8.GetBytes("\n>>AUTOMOBIL: vracam se u garazu!!\n");
                            automobilTCPSocket.Send(porukaDirekciji);
                            automobilTCPSocketDirekcija.Send(porukaDirekciji);
                            break;
                        }
                    }
                }

                int porukaBr = Int32.Parse(porukeGraze);

                automobil.gumeAutomobila.duzinaKoriscenja = automobil.gumeAutomobila.duzinaKoriscenja - (int)(staza.duzinaStaze * automobil.konfiguracijaAutomobila.potrosnjaGuma);
                automobil.kolicinaGoriva = automobil.kolicinaGoriva - (int)(staza.duzinaStaze * automobil.konfiguracijaAutomobila.potrosnjaGoriva);

                double tempoGuma = 0, tempoGoriva = 0;

                tempoGoriva = 1.0 / automobil.kolicinaGoriva;

                if (automobil.gumeAutomobila.duzinaKoriscenja < automobil.gumeAutomobila.duzinaKoriscenja * 0.4)
                {
                    tempoGuma = 0.6 * (n + 1);
                }
                else
                {
                    switch (automobil.gumeAutomobila.gumaType)
                    {
                        case Klase.GumaType.M:
                            tempoGuma = 1.2 * (n + 1);
                            break;

                        case Klase.GumaType.T:
                            tempoGuma = 0.8 * (n + 1);
                            break;

                        case Klase.GumaType.S:
                            tempoGuma = n + 1;
                            break;
                    }
                }

                if (porukaBr == 1)
                {
                    tempoGoriva += 0.3;
                    tempoGuma += 0.3;
                    vremeKruga = staza.vremeTrajanjaKruga - tempoGoriva - tempoGuma;
                }
                else if (porukaBr == 2)
                {

                    vremeKruga = staza.vremeTrajanjaKruga - tempoGoriva - tempoGuma + 0.2 * brojacSporeVoznje;
                    brojacSporeVoznje++;
                }
                else
                {
                    vremeKruga = staza.vremeTrajanjaKruga - tempoGoriva - tempoGuma;
                }

                if (automobil.gumeAutomobila.duzinaKoriscenja <= pocetnaVrednostGuma * 0.15 || automobil.kolicinaGoriva < tempoGoriva * staza.duzinaStaze)
                {
                    automobilTCPSocket.Send(Encoding.UTF8.GetBytes("\n>>AUTOMOBIL: vrednosti guma ili goriva manje od bezbednih povratak u garazu!!\n"));

                    automobilTCPSocketDirekcija.Send(Encoding.UTF8.GetBytes("\n>>AUTOMOBIL: vrednosti guma ili goriva manje od bezbednih povratak u garazu!!\n"));

                    break;
                }

                //SALJE DIREKCIJI TRKE SVAKO VREME KRUGA

                byte[] vremeZaSlanje = new byte[1024];

                vremeZaSlanje = Encoding.UTF8.GetBytes(vremeKruga.ToString() + "/" + automobil.trkackiBroj + " " + automobil.konfiguracijaAutomobila.ime);

                automobilTCPSocketDirekcija.Send(vremeZaSlanje);

                vremenaPoKrugu.Add(vremeKruga);

                //SLANJE POTROSNJE GUMA I GORIVA GARAZI

                byte[] poruka = Encoding.UTF8.GetBytes($"\n>>Potrosnja: Guma: {Math.Round(tempoGuma, 2)} Gorivo: {Math.Round(tempoGoriva, 4)}");

                automobilTCPSocket.Send(poruka);

                Console.Write($">>Krug {n + 1}. : " + Math.Round(vremeKruga, 2) + "\n");

                Thread.Sleep((int)vremeKruga * 1000);

                n++;

            } while (n < 11);

            return vremenaPoKrugu;

        }

    }
}
