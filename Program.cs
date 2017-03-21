using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TipClicker
{
    class Program
    {
        public static float version = 1.05f;

        static void Main(string[] args)
        {
            Manager.Start();
            Console.ReadLine();
        }
    }
}
