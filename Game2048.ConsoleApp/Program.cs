using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game2048.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            GameModel gm = new GameModel(4, 2048);
            Console.ReadLine();
        }


        /*
        0 2 2      2 0 0
        2 4 0  =>  2 4 2
        0 2 0      0 2 0
        */
        public static int[,] RotatePositive90Deg(int[,] arr2D)
        {
            int rowLength = arr2D.GetLength(0);
            int colLength = arr2D.GetLength(1);
            int[,] temp = new int[rowLength, colLength];

            for (int i = 0; i < rowLength; i++)
            {
                for (int j = 0; j < colLength; j++)
                {
                    int newX = j;
                    int newY = rowLength - i - 1;
                    temp[i, j] = arr2D[newX, newY];
                }
            }
            return temp; 
        }

        public static int[,] RotateNegative90Deg(int[,] arr2D)
        {
            int rowLength = arr2D.GetLength(0);
            int colLength = arr2D.GetLength(1);
            int[,] temp = new int[rowLength, colLength];

            for (int i = 0; i < rowLength; i++)
            {
                for (int j = 0; j < colLength; j++)
                {
                    int newX = rowLength - j - 1;
                    int newY = i;
                    temp[i, j] = arr2D[newX, newY];
                }
            }
            return temp;
        }



    }
}
