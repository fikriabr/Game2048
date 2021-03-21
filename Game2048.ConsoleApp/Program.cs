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
    }
}
