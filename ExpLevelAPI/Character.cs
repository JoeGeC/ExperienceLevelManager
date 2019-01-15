using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpLevelAPI
{
    //Used for calculating exp-level formula
    enum NextOperator
    {
        eAdd,
        eSubtract,
        eTimes,
        eDivide,
        ePower
    }

    class Character
    {
        private string formula;
        private NextOperator nextOperator = NextOperator.eAdd;

        public long Experience { get; set; }

        public long Level
        {
            get
            {
                long level = Experience;
                string tempNum = "0";
                //loops through inputted formula and calculates current level from it
                foreach (char i in formula)
                {
                    switch (i)
                    {
                        case '+':
                            PerformNextOperation(ref tempNum, ref level);
                            nextOperator = NextOperator.eAdd;
                            break;
                        case '-':
                            PerformNextOperation(ref tempNum, ref level);
                            nextOperator = NextOperator.eSubtract;
                            break;
                        case '*':
                            PerformNextOperation(ref tempNum, ref level);
                            nextOperator = NextOperator.eTimes;
                            break;
                        case '/':
                            PerformNextOperation(ref tempNum, ref level);
                            nextOperator = NextOperator.eDivide;
                            break;
                        case '^':
                            PerformNextOperation(ref tempNum, ref level);
                            nextOperator = NextOperator.ePower;
                            break;
                        case ' ':
                            break;
                        default:
                            if (char.IsDigit(i))
                            {
                                tempNum = tempNum + i.ToString();
                            }
                            else
                            {
                                throw new System.ArgumentException("Invalid formula. Please use only digits (0-9) and operators (+, -, /, *, ^)");
                            }
                            break;
                    }

                }
                PerformNextOperation(ref tempNum, ref level);
                return level;
            }
            set
            {
                Level = value;
            }
        }

        //Used when calculating levels in Level getter
        private void PerformNextOperation(ref string tempNum, ref long level)
        {
            if (tempNum != "0")
            {
                switch (nextOperator)
                {
                    case NextOperator.eAdd:
                        level += Convert.ToInt64(tempNum);
                        break;
                    case NextOperator.eSubtract:
                        level -= Convert.ToInt64(tempNum);
                        break;
                    case NextOperator.eDivide:
                        level /= Convert.ToInt64(tempNum);
                        break;
                    case NextOperator.eTimes:
                        level *= Convert.ToInt64(tempNum);
                        break;
                    case NextOperator.ePower:
                        level = (long)Math.Pow(level, Convert.ToInt64(tempNum));
                        break;
                    default:
                        throw new System.ArgumentException("No operator found in formula");
                }

                tempNum = "0";
            }
        }

        //TODO:: WITH FORMULA
        //Returns how much exp is needed for level
        public long GetExperienceValue(long level)
        {
            return level * 10;
        }

        //TODO:: WITH FORMULA
        //Returns what level character will be at with plugged in exp
        public long GetLevelValue(long experience)
        {
            return experience / 10;
        }

        //TODO
        //Returns how much exp is needed for level up
        public long GetNextLevelUp()
        {
            return 0;
        }

        //TODO
        //Returns percentage of how much progress has been made to next level
        public float GetExpProgressToNextLevel()
        {
            return 0;
        }

        //TODO
        //Returns experience delta between current exp and plugged in level
        public long GetExpLevelDelta(long level)
        {
            return 0;
        }

        //To subtract, plug in a minus number
        public void AddExperience(long exp)
        {
            Experience += exp;
        }

        //Resets exp to last level up
        //TODO
        public void ResetExp()
        {

        }

        //Formula is in the form - Level = Experience "your formula here". This does not follow BIDMAS rules
        //i.e. input "/ 10 * 3"
        public Character(string form)
        {
            formula = form;
        }
    }
}
