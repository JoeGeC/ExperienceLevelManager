using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpLevelAPI
{
    class Program
    {
        static void Main(string[] args)
        {
            Character char1 = new Character(new List<string>() { "Exp", "/", "2", "+", "5" });
            char1.Experience = 10;
            Console.WriteLine(char1.Level);
            Console.ReadLine();


            

        }
    }
}
