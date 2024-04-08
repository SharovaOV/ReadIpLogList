using System.Net;

namespace ReadIpLogList
{
    class IpRangeSegment:IpRangeReduce
    {
        byte[] ipEndBts;
        public IpRangeSegment(Parametrs parametrs) : base(parametrs)
        {

            byte[] maskBts = parametrs.AddressMask.GetAddressBytes();
            ipEndBts = new byte[ipStartBts.Length];
            byte dif = 0, temp, bt = 255;
            for (int i = 0; i < ipEndBts.Length; i++)
            {
                temp = (byte)(bt - maskBts[i]);
                dif = (byte)(bt - ipStartBts[i]);
                dif = (dif < temp) ? dif : temp;
                ipEndBts[i] = (byte)(ipStartBts[i] + dif);
            }
        }

        protected override bool ParseIp(string str)
        {
            IPAddress currAddres;
            if (Parametrs.RegxIP.IsMatch(str) && IPAddress.TryParse(str, out currAddres))
            {
                var curBts = currAddres.GetAddressBytes();
                for (int i = 0; i < curBts.Length; i++)
                {
                    if (curBts[i] < ipStartBts[i] || curBts[i] > ipEndBts[i])
                        return false;
                }
            }
            return true;
        }
    }
}
