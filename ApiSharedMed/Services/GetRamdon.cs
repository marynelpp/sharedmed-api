using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiSharedMed.Services
{
    public class GetRamdon
    {
        internal static string GetRandomAlphaNumeric(int id1 = 0, int id2 = 0, int length = 8)
        {
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            var list = Enumerable.Repeat(0, length).Select(x => chars[random.Next(chars.Length)]);
            var outVal = string.Join("", list);
            if (id1 != 0)
            {
                outVal = outVal.Insert(2, (id1 + 3).ToString());
            }
            if (id2 != 0)
            {
                outVal = outVal.Insert(6, (id2 + 1).ToString());
            }
            return outVal;
        }
    }
}
