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
            //Character char1 = new Character("/ 10");
            //char1.Experience = 10;
            //Console.WriteLine(char1.Experience);
            //Console.WriteLine(char1.Level);
            //Console.WriteLine(char1.Level);
            //char1.Experience = 20;
            //Console.WriteLine(char1.Level);
            //Console.WriteLine(char1.GetNextLevelUp());
            //Console.ReadLine();

            //List<string> char1Formula = new List<string>();
            //char1Formula.Add()
            Character1 char1 = new Character1(new List<string>() { "10", "/", "2", "*", "(", "3", "+", "5", ")", "*", "13" });
            Console.WriteLine(char1.FormulaCalc());
            Console.ReadLine();


            

        }
    }
}
