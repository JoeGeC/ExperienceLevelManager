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
            Character char1 = new Character(new List<string>() { "Exp", "/", "10", "+", "8" });
            char1.Experience = 92;
            Console.WriteLine(char1.Level);
            Console.WriteLine(char1.GetLevelExp(10));
            Console.ReadLine();


            

        }
    }
}
