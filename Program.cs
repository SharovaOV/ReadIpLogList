using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;

namespace ReadIpLogList
{
    class Program
    {
        static void Main(string[] args)
        {
            Parametrs parametrs;
            parametrs = new Parametrs(args);
            Console.WriteLine($"Path file for load: {parametrs.FileLog}");
            Console.WriteLine($"Path file for output: {parametrs.FileOutput}");
            Console.WriteLine($"Start IP for output: {parametrs.AddressStart}");
            Console.WriteLine($"Netmask for output: {parametrs.AddressMask}");
            Console.WriteLine($"Range start date for output: {parametrs.TimeStart:dd.MM.yyyy}"); 
            Console.WriteLine($"Range end date for output: {parametrs.TimeEnd:dd.MM.yyyy}");
            ListRange.Create(parametrs);
            WaitingKey();
        }
        static void WaitingKey()
        {
            Console.WriteLine("Press any key for exit...");
            Console.ReadKey();
        }
    }
}
