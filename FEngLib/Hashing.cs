using System.Linq;

namespace FEngLib
{
    public static class Hashing
    {
        public static uint BinHash(string str)
        {
            return str.Aggregate(0xFFFFFFFFu, (h, c) => h * 33 + c);
        }
    }
}