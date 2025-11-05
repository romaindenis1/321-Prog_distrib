using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ntp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string[] ntpServers = { "0.ch.pool.ntp.org", "1.ch.pool.ntp.org", "time.google.com"};
            
            var dateTime = DateTime.Now;
            Console.WriteLine("Machine Time : ");
            CalcDate(dateTime);

            /*
             * Si networkDateTime n'est pas specifié avec SpecifyKind et DateTimeKind.Utc
             * .NET assume que la date est local (UTC+1 au lieu de UTC+0)
             * Donc return fausse valeure
             */
            foreach (string server in ntpServers)
            {
                byte[] timeMessage = new byte[48];
                timeMessage[0] = 0x1B;
                IPEndPoint ntpReference = new IPEndPoint(Dns.GetHostAddresses(server)[0], 123);

                UdpClient client = new UdpClient();
                client.Connect(ntpReference);
                client.Send(timeMessage, timeMessage.Length);
                timeMessage = client.Receive(ref ntpReference);

                ulong intPart = (ulong)timeMessage[40] << 24 | (ulong)timeMessage[41] << 16 | (ulong)timeMessage[42] << 8 | (ulong)timeMessage[43];
                ulong fractPart = (ulong)timeMessage[44] << 24 | (ulong)timeMessage[45] << 16 | (ulong)timeMessage[46] << 8 | (ulong)timeMessage[47];

                var milliseconds = (intPart * 1000) + ((fractPart * 1000) / 0x100000000L);
                var networkDateTime = DateTime.SpecifyKind(
                (new DateTime(1900, 1, 1)).AddMilliseconds((long)milliseconds),
                    DateTimeKind.Utc
                );

                Console.WriteLine($"{server} Time");
                CalcDate(networkDateTime);

                client.Close();
            }            
        }
        static void CalcDate(DateTime x)
        {
            Console.WriteLine($"Heure Default : {x}");
            Console.WriteLine($"Heure UTC : {x.ToUniversalTime()}");
            Console.WriteLine($"Heure actuelle Long : {x.ToLongDateString()}");
            Console.WriteLine($"Heure actuelle Courte : {x.ToShortDateString()}");
            Console.WriteLine($"Heure ISO 8601 : {x.ToString("o", CultureInfo.InvariantCulture)} ");
        }
    }
}
