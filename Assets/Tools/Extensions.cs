using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssemblyCSharp.Assets.Tools
{
    public static class Extensions
    {
        private static Random rng = new Random();

        public static T RandomOne<T>(this T[] array)
        {
            return array[rng.Next(array.Length)];
        }

        public static (T Element, int Index) RandomOneWithIndex<T>(this T[] array)
        {
            int index = rng.Next(array.Length);
            return (array[index], index);
        }
    }
}
