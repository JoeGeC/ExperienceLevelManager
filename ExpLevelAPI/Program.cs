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
            Character char1 = new Character(new List<string>() { "exp", "/", "(", "2", "+", "3", ")", "^", "2" });
            char1.Experience = 100;
            Console.WriteLine(char1.Level);
            Console.WriteLine(char1.GetLevelExp(10));
            Console.ReadLine();


            

        }
    }
}
