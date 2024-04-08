using System;

namespace ReadIpLogList
{
    static class ListRange
    {
        static public void Create(Parametrs parametrs)
        {
            IpRange range;
            if (parametrs.AddressStart == null)
                range = new IpRange(parametrs);
            else if (parametrs.AddressMask == null)
                range = new IpRangeReduce(parametrs);
            else
                range = new IpRangeSegment(parametrs);

            range.ReadFile();
            range.WriteFile();
        }
    }


}
