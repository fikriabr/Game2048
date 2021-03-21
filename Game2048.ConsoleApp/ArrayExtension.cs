using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Game2048.ConsoleApp
{
    public static class ArrayExtension
    {
        public static T[] GetRow<T> (this T[,] array, int rowNumber)
        {
            return Enumerable.Range(0, array.GetLength(1))
               .Select(x => array[rowNumber, x])
               .ToArray();
        }

        public static T[] GetCol<T>(this T[,] array, int colNumber)
        {
            return Enumerable.Range(0, array.GetLength(0))
               .Select(x => array[x, colNumber])
               .ToArray();

        }

    }
}
