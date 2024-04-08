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
            Console.WriteLine($"file for load: {parametrs.FileLog}");
            Console.WriteLine($"file for output: {parametrs.FileOutput}");
            Console.WriteLine($"startIp for output: {parametrs.AddressStart}");
            Console.WriteLine($"maskIp for output: {parametrs.AddressMask}");
            Console.WriteLine($"startDate for output: {parametrs.TimeStart:dd:MM:yyyy}"); 
            Console.WriteLine($"endDate for output: {parametrs.TimeEnd:dd:MM:yyyy}");
            ListRange.Create(parametrs);
            
            weightKey();
        }
        static void weightKey()
        {
            Console.WriteLine("Нажмите enter для выхода...");
            Console.Read();

        }
    }
}
