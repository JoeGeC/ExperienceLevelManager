using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpLevelAPI
{
    class Character
    {
        private List<string> lvlFormula;
        private List<string> expFormula = new List<string>();
        public long Experience { get; set; }
        public long Level
        {
            get
            {
                return FormulaCalc(lvlFormula);
            }
            set
            {
                Level = value;
            }
        }

        //"experience" or "exp" for experience variable.
        //if using power, brackets should be used. i.e. "(", "3", "^", "2", ")"
        //"root" for root
        public Character(List<string> form)
        {
            lvlFormula = form;
        }

        //reverse polish notation calculator
        private long RPNCalculator(Queue<string> outputQueue)
        {
            Stack<double> rpnStack = new Stack<double>();

            foreach (string i in outputQueue)
            {
                double popNum = 0;
                //if number, push to stack
                if (double.TryParse(i, out double n))
                {
                    rpnStack.Push(Convert.ToDouble(i));
                }
                //if operator, pop values from stack, perform operation on those values and push back result
                else if (i == "+")
                {
                    rpnStack.Push(rpnStack.Pop() + rpnStack.Pop());
                }
                else if (i == "-")
                {
                    popNum = rpnStack.Pop();
                    rpnStack.Push(rpnStack.Pop() - popNum);
                }
                else if (i == "*")
                {
                    popNum = rpnStack.Pop();
                    rpnStack.Push(rpnStack.Pop() * popNum);
                }
                else if (i == "/")
                {
                    popNum = rpnStack.Pop();
                    rpnStack.Push(rpnStack.Pop() / popNum);
                }
                else if (i == "^")
                {
                    popNum = rpnStack.Pop();
                    rpnStack.Push(Math.Pow(rpnStack.Pop(), popNum));
                }
                else if (i == "root")
                {
                    popNum = rpnStack.Pop();
                    rpnStack.Push(Math.Pow(rpnStack.Pop(), (1 / popNum)));
                }
            }
            //if formula inputted wrong, rpnStack might end up with more than 1 value left
            if (rpnStack.Count > 1)
            {
                throw new System.InvalidOperationException("Too many values left in stack. Please check formula input.");
            }
            return (long)rpnStack.Pop();
        }

        //calculates level from formula passed into constructor
        private long FormulaCalc(List<string> formula)
        {
            //Shunting-yard algorithm
            Queue<string> outputQueue = new Queue<string>();
            Stack<string> operatorStack = new Stack<string>();

            foreach (string i in formula)
            {
                //if token is a number, push it to output queue
                if (i.ToLower() == "experience" || i.ToLower() == "exp")
                {
                    outputQueue.Enqueue(Experience.ToString());
                }
                else if (double.TryParse(i, out double n))
                {
                    outputQueue.Enqueue(i);
                }
                //if token is an operator, push it to operator stack
                else if (i == "+" || i == "-")
                {
                    //while there is an operator at top of stack with greater precedence or equal precedence and is left associative, and not a left bracket, pop from operator stack to output queue
                    while (operatorStack.Count != 0 && (operatorStack.Peek() == "/" || operatorStack.Peek() == "*" || operatorStack.Peek() == "^"  || operatorStack.Peek() == "root" || operatorStack.Peek() == "+"
                        || operatorStack.Peek() == "-" && operatorStack.Peek() != "("))
                    {
                        outputQueue.Enqueue(operatorStack.Pop());
                    }

                    operatorStack.Push(i);
                }
                else if (i == "/" || i == "*")
                {
                    while (operatorStack.Count != 0 && (operatorStack.Peek() == "^" || operatorStack.Peek() == "root" || operatorStack.Peek() == "/" || operatorStack.Peek() == "*" && operatorStack.Peek() != "("))
                    {
                        outputQueue.Enqueue(operatorStack.Pop());
                    }

                    operatorStack.Push(i);
                }
                else if (i == "^" || i == "root")
                {
                    operatorStack.Push(i);
                }
                //if token is left bracket, push to operator stack
                else if (i == "(")
                {
                    operatorStack.Push(i);
                }
                //if token is right bracket...
                else if (i == ")")
                {
                    //while operator at top of stack is not left bracket, pop operator from operator stack to output queue
                    while (operatorStack.Count != 0 && operatorStack.Peek() != "(")
                    {
                        outputQueue.Enqueue(operatorStack.Pop());
                    }
                    //check for mismatched brackets and pop left bracket from stack
                    if (operatorStack.Count != 0 && operatorStack.Peek() == "(")
                    {
                        operatorStack.Pop();
                    }
                    else
                    {
                        throw new System.InvalidOperationException("Mismatched brackets. Please check formula input.");
                    }
                }
                else
                {
                    throw new System.InvalidOperationException($"You cannot input '{i}' to the formula. Please check formula input.");
                }
            }
            //if operator on top of stack is bracket, mismatched brackets
            if(operatorStack.Peek() == "(" || operatorStack.Peek() == ")")
            {
                throw new System.InvalidOperationException("Mismatched brackets. Please check formula input.");
            }
            //if there are stil operators on stack, pop from operator stack onto output queue
            while (operatorStack.Count != 0)
            {
                outputQueue.Enqueue(operatorStack.Pop());
            }

            return RPNCalculator(outputQueue);
        }

        public long GetLevelExp(long lvl)
        {
            return RearrangeEquation(lvl, lvlFormula);
        }

        //TODO:: DOES NOT WORK IF EXP IS INSIDE BRACKETS
        //calculates how much exp is needed for a level that is passed in
        private long RearrangeEquation(long lvl, List<string> formula, bool inBrackets = false)
        {
            //temp lvl formula so we can perform calculations on it without affecting original formula
            List<string> tmpLvlFormula = formula;

            //performs calculations within brackets
            for (int i = 0; i < tmpLvlFormula.Count - 1; i++)
            {
                if(tmpLvlFormula[i] == "(")
                {
                    int brackCount = 1;
                    for (int j = i + 1; j < tmpLvlFormula.Count - 1; j++)
                    {
                        if (tmpLvlFormula[j] == "(")
                            brackCount++;
                        else if (tmpLvlFormula[j] == ")")
                        {
                            brackCount--;
                            if (brackCount == 0)
                            {
                                List<string> bracketFormula = new List<string>();
                                tmpLvlFormula.RemoveAt(i);
                                for(int k = i; k < j - 1; k++)
                                {
                                    bracketFormula.Insert(bracketFormula.Count, tmpLvlFormula[i]);
                                    tmpLvlFormula.RemoveAt(i);
                                }
                                tmpLvlFormula.RemoveAt(i);
                                tmpLvlFormula.Insert(i, RearrangeEquation(Convert.ToInt64(bracketFormula[0]), bracketFormula, true).ToString());
                                expFormula.Clear();
                            }
                        }
                    }
                }
            }

            expFormula.Insert(0, lvl.ToString()); //insert lvl value at start of formula

            for (int i = tmpLvlFormula.Count - 1; i > 0; i--)
            {
                if (tmpLvlFormula[i] == "+" || tmpLvlFormula[i] == "-")
                {
                    if(inBrackets)
                    {
                        expFormula.Insert(expFormula.Count, tmpLvlFormula[i]);
                        expFormula.Insert(expFormula.Count, tmpLvlFormula[i + 1]);
                    }
                    else
                    {
                        switch(tmpLvlFormula[i])
                        {
                            case ("+"):
                                expFormula.Insert(expFormula.Count, "-");
                                break;
                            case ("-"):
                                expFormula.Insert(expFormula.Count, "+");
                                break;
                            default:
                                throw new System.InvalidOperationException("i does not equal '-' or '+'.");
                        }
                        expFormula.Insert(expFormula.Count, tmpLvlFormula[i + 1]);
                    }
                }
                else if(tmpLvlFormula[i] == "/" || tmpLvlFormula[i] == "*" || lvlFormula[i] == "^" || lvlFormula[i] == "root")
                {
                    //if connected to exp, insert into new formula and calculate against whole equation
                    if(tmpLvlFormula[i - 1].ToLower() == "exp" || tmpLvlFormula[i - 1].ToLower() == "experience")
                    {
                        expFormula.Insert(0, "(");
                        expFormula.Insert(expFormula.Count, ")");
                        switch(tmpLvlFormula[i])
                        {
                            case ("/"):
                                expFormula.Insert(expFormula.Count, "*");
                                break;
                            case ("*"):
                                expFormula.Insert(expFormula.Count, "/");
                                break;
                            case ("^"):
                                expFormula.Insert(expFormula.Count, "root");
                                break;
                            case ("root"):
                                expFormula.Insert(expFormula.Count, "^");
                                break;
                        }                        
                        expFormula.Insert(expFormula.Count, tmpLvlFormula[i + 1]);
                    }
                    //if not connected to exp, do calculation now
                    else
                    {
                        switch (tmpLvlFormula[i])
                        {
                            case ("/"):
                                tmpLvlFormula[i - 1] = (Convert.ToDouble(tmpLvlFormula[i - 1]) / Convert.ToDouble(tmpLvlFormula[i + 1])).ToString();
                                break;
                            case ("*"):
                                tmpLvlFormula[i - 1] = (Convert.ToDouble(tmpLvlFormula[i - 1]) * Convert.ToDouble(tmpLvlFormula[i + 1])).ToString();
                                break;
                            case ("^"):
                                tmpLvlFormula[i - 1] = (Math.Pow(Convert.ToDouble(tmpLvlFormula[i - 1]), Convert.ToDouble(tmpLvlFormula[i + 1]))).ToString();
                                break;
                            case ("root"):
                                tmpLvlFormula[i - 1] = (Math.Pow(Convert.ToDouble(tmpLvlFormula[i - 1]), 1 / Convert.ToDouble(tmpLvlFormula[i + 1]))).ToString();
                                break;
                        }
                    }
                }
            }

            return FormulaCalc(expFormula);
        }
    }
}
