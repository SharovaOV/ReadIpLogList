using System.Net;

namespace ReadIpLogList
{
    class IpRangeReduce:IpRange
    {
        protected byte[] ipStartBts;
        public IpRangeReduce(Parametrs parametrs) : base(parametrs)
        {
            ipStartBts = parametrs.AddressStart.GetAddressBytes();
        }
        protected override bool ParseIp(string str)
        {
            IPAddress curAddress;
            if (base.ParseIp(str) && IPAddress.TryParse(str, out curAddress))
            {
                var bts = curAddress.GetAddressBytes();
                for (int i = 0; i < bts.Length; i++)
                {
                    if (bts[i] < ipStartBts[i])
                        return false;
                }
                return true;
            }
            return false;
        }

    }
}
