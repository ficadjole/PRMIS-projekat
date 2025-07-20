using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Garaza.Services
{
    class SlanjeDirektiva
    {

        public void slanjeDirektiva(Socket garazaUDPSoket, ref EndPoint garazaUDPPoint, ref bool prekid)
        {
            string odabir = null;

            while (true)
            {


                Console.WriteLine("\nOdaberite tempo:\n1. Brzi tempo\n2. Sporiji tempo\n3. Srednji tempo\n4. Povratak automobila u garazu");
                Console.Write("Vas odabir: ");

                odabir = Console.ReadLine();

                byte[] odgovor = new byte[1024];

                switch (Int32.Parse(odabir))
                {

                    case 1:
                        odgovor = Encoding.UTF8.GetBytes("1");

                        garazaUDPSoket.SendTo(odgovor, 0, odgovor.Length, SocketFlags.None, garazaUDPPoint);

                        break;

                    case 2:

                        odgovor = Encoding.UTF8.GetBytes("2");

                        garazaUDPSoket.SendTo(odgovor, 0, odgovor.Length, SocketFlags.None, garazaUDPPoint);

                        break;

                    case 3:

                        odgovor = Encoding.UTF8.GetBytes("3");

                        garazaUDPSoket.SendTo(odgovor, 0, odgovor.Length, SocketFlags.None, garazaUDPPoint);
                        break;

                    case 4:

                        byte[] odgovorByte = Encoding.UTF8.GetBytes("da");

                        int brbajta = garazaUDPSoket.SendTo(odgovorByte, 0, odgovorByte.Length, SocketFlags.None, garazaUDPPoint);

                        prekid = true;
                        
                        break;

                    default:

                        odgovor = Encoding.UTF8.GetBytes("0");

                        garazaUDPSoket.SendTo(odgovor, 0, odgovor.Length, SocketFlags.None, garazaUDPPoint);

                        break;

                }

            }
        }

    }
}
