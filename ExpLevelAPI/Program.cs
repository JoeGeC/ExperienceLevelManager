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
            Character char1 = new Character(new List<string>() { "exp", "/", "(", "(", "5", "-", "4", ")", "+", "3", ")", "^", "2" });
            char1.Experience = 112;
            Console.WriteLine("Level: " + char1.Level);
            Console.WriteLine("Exp needed for level 10: " + char1.GetLevelExp(10));
            Console.WriteLine("Exp needed for next level: " + char1.GetNextLevelExp());
            Console.WriteLine("Exp delta for level 10: " + char1.GetExpDelta(10));
            Console.WriteLine("Progress to next level: " + char1.GetProgressToNextLevel() + "%");
            Console.ReadLine();
        }
    }
}
