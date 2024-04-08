using System;
using System.Collections.Generic;
using System.IO;
namespace ReadIpLogList
{
    class IpRange
    {
        Dictionary<string, int> inRange;
        HashSet<string> outsider;
        string fileOutput, fileLog;
        DateTime dtStart, dtEnd;

        string separator = ":";
        public IpRange(Parametrs parametrs)
        {
            inRange = new Dictionary<string, int>();
            outsider = new HashSet<string>();
            fileOutput = parametrs.FileOutput;
            fileLog = parametrs.FileLog;
            dtStart = parametrs.TimeStart;
            dtEnd = parametrs.TimeEnd;
        }

        public void Add(string key)
        {
            if (!inRange.ContainsKey(key))
            {
                if (outsider.Contains(key)) return;
                if (!ParseIp(key))
                {
                    outsider.Add(key);
                    return;
                }
                inRange[key] = 0;
            }
            inRange[key]++;
        }

        public void WriteFile()
        {
            Console.WriteLine("Start Write file!");
            using (StreamWriter writer = new StreamWriter(fileOutput, false))
            {
                foreach (var rng in inRange)
                {
                    writer.WriteLine($"{rng.Key}{separator}{rng.Value}");
                }
            }
        }

        public void ReadFile()
        {
            Console.WriteLine("Start read file!");

            using (StreamReader reader = new StreamReader(fileLog))
            {

                while (!reader.EndOfStream)
                {
                    if (!ReadLine(reader.ReadLine()))
                        break;
                }
            }
            Console.WriteLine("Finish read file!");
        }

        bool ReadLine(string str)
        {
            string[] ipAddressDate = str.Split(new char[] { ':' }, 2, StringSplitOptions.RemoveEmptyEntries);
            DateTime dt;
            DateTime.TryParseExact(ipAddressDate[1], "yyyy-MM-dd HH:mm:ss", Parametrs.Provider, System.Globalization.DateTimeStyles.None, out dt);
            if (dt != null)
            {
                if (dt >= dtStart && dt <= dtEnd)
                {
                    Add(ipAddressDate[0]);
                    return true;
                }
                else if (dt > dtEnd) return false;
            }
            return true;
        }


        protected virtual bool ParseIp(string str)
        {
            return Parametrs.RegxIP.IsMatch(str);
        }
    }
}
