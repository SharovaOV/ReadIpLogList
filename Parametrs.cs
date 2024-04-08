using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Net;
using System.Configuration;
using System.Globalization;
using System.Text.RegularExpressions;
namespace ReadIpLogList
{
    class Parametrs
    {
        enum TypeVar
        {
            FileLog = 0,
            FileOutput,
            AdressStart,
            AdressMask,
            TimeStart,
            TimeEnd
        };

        public string FileLog { get => fileLog; }
        public string FileOutput { get => fileOutput; }
        public IPAddress AddressStart { get => addressStart; }
        public IPAddress AddressMask { get => addressMask; }
        public DateTime TimeStart { get => timeStart; }
        public DateTime TimeEnd { get => timeEnd; }
        public static CultureInfo Provider { get => provider; }
        public static Regex RegxIP { get => regxIP; }



        private HashSet<char> _invalidCharacters = new HashSet<char>(Path.GetInvalidPathChars());
        static CultureInfo provider = CultureInfo.InvariantCulture;
        static Regex regxIP = new Regex(@"^\b(?:(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.){3}(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\b$");
        Regex regxIPMask = new Regex(@"/^(((255\.){3}(255|254|252|248|240|224|192|128|0+))|((255\.){2}(255|254|252|248|240|224|192|128|0+)\.0)|((255\.)(255|254|252|248|240|224|192|128|0+)(\.0+){2})|((255|254|252|248|240|224|192|128|0+)(\.0+){3}))$/");

       
        string fileLog= "newlogfile.log", fileOutput = "outip.txt";
        IPAddress addressStart, addressMask;
        DateTime timeStart = new DateTime(2022,1,1), timeEnd = new DateTime(2023,1,1);        

        public Parametrs(string[] args) : this()
        {
            SetParametrs(args);
        }

        public Parametrs()
        {
            if (!File.Exists(fileLog))
                File.Create(fileLog);
            var configs = ConfigurationManager.AppSettings;

            SetFilLog(configs[(int)TypeVar.FileLog]);
            SetFileOutput(configs[(int)TypeVar.FileOutput]);

            SetAdressStart(configs[(int)TypeVar.AdressStart]);
            SetAdressMask(configs[(int)TypeVar.AdressMask]);

            SetDateStart(configs[(int)TypeVar.TimeStart]);
            SetDateEnd(configs[(int)TypeVar.TimeEnd]);

        }

        public void SetParametrs(string[] args)
        {
            int currType = 0, maxType = (int)TypeVar.TimeEnd; 
            for (int i = 0; i < args.Length; i++)
            {
                bool isValid = false;
                
                while (!isValid && currType <= maxType)
                {
                    isValid = ParseType((TypeVar)currType, args[i]);
                    currType++;
                }
                if(currType > maxType)
                    currType = i+1;
            }
        }

        bool ParseType(TypeVar type, string value)
        {
            if (value == "def") return true;
            switch (type)
            {
                default: return false;

                case TypeVar.FileLog:                    
                    return SetFilLog(value);

                case TypeVar.FileOutput:
                    return SetFileOutput(value);

                case TypeVar.AdressStart:
                    return SetAdressStart(value);

                case TypeVar.AdressMask:
                    return SetAdressMask(value);

                case TypeVar.TimeStart:
                    return SetDateStart(value);
                    
                case TypeVar.TimeEnd:
                    return SetDateEnd(value);
            }
        }
        

        bool IsPathValid(string filePath)
        {
            return !string.IsNullOrWhiteSpace(filePath) &&
                    !filePath.ToCharArray().Any(pc => _invalidCharacters.Contains(pc)) &&
                    !DateTime.TryParseExact(filePath, "dd.MM.yyyy",provider,DateTimeStyles.None,out var dt) &&
                    !regxIP.IsMatch(filePath);
        }

        bool IsPathValidAndExists(string filePath)
        {
            return IsPathValid(filePath) && File.Exists(filePath);
        }
        bool TryParsedIpMask(string ipAdress, out IPAddress address)
        {
            address = null;
            return regxIPMask.IsMatch(ipAdress) && IPAddress.TryParse(ipAdress, out address); ;
        }
        public static bool TryParsedIp(string ipAdress, out IPAddress address)
        {
            address = null;
            return regxIP.IsMatch(ipAdress) && IPAddress.TryParse(ipAdress, out address);
        }

        public static bool TryParseDateTime(string value, ref DateTime tm)
        {
            bool ans = DateTime.TryParseExact(value, "dd.MM.yyyy", provider, DateTimeStyles.None, out var tempDate);
            tm = ans ? tempDate : tm;
            return ans;
        }


        bool SetFilLog(string value)
        {
            if (IsPathValidAndExists(value))
            {
                fileLog = value;
                return true;
            }
            return false;
        }

        bool SetFileOutput(string value)
        {
            if (IsPathValid(value))
            {
                fileOutput = value;
                return true;
            }
            return false;
        }

        bool SetAdressStart(string value)
        {
            return TryParsedIp(value, out addressStart);
        }

        bool SetAdressStart(IPAddress address)
        {
            addressStart = address;
            return true;
        }

        bool SetAdressMask(string value)
        {
            if(!TryParsedIpMask(value, out addressMask)){
                if(!string.IsNullOrEmpty(value) && int.TryParse(value, out var num))
                {
                    addressMask = getMask(num);
                }

            }

            return addressMask != null;
        }

        IPAddress getMask(int numMask)
        {
            if (numMask > 32) return null;

            byte[] bts = new byte[4];
            int sizeFill = numMask / 8, i=0;
            for(;i< sizeFill; i++)
            {
                bts[i] = 255;
            }

            if (i < bts.Length && (numMask % 8 > 0))
            {
                bts[i] = (byte)(256 - (1 << (8 - numMask % 8)));
            }

            return new IPAddress(bts);
        }

        bool SetDateStart(string value)
        {
            bool res =  TryParseDateTime(value, ref timeStart);
            if (timeEnd != null)
                CorrectTimeEnd();
            return res;
        }
        void CorrectTimeEnd()
        {
            if (timeStart > timeEnd)
                timeEnd = timeStart;
        }
        bool SetDateEnd(string value)
        {
            if( TryParseDateTime(value, ref timeEnd))
            {
                CorrectTimeEnd();
                return true;
            }
            CorrectTimeEnd();
            return false;
        }

    }
}
