using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpLevelAPI
{
    class Character
    {
        private List<string> formula;
        public long Experience { get; set; }
        public long Level
        {
            get
            {
                return FormulaCalc();
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
            formula = form;
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
        private long FormulaCalc()
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
                    while (operatorStack.Count != 0 && (operatorStack.Peek() != "("))
                    {
                        outputQueue.Enqueue(operatorStack.Pop());
                    }

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
                    //while operator at top of sack is not left bracket, pop operator from operator stack to output queue
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


        //TODO:: DOES NOT WORK
        //calculates how much exp is needed for a level that is passed in
        public long GetLevelExp(long lvl)
        {
            //Shunting-yard algorithm
            List<string> outputQueue = new List<string>();
            Stack<string> operatorStack = new Stack<string>();
            List<string> multDivQueue = new List<string>();

            for (int i = 0; i < formula.Count; i++)
            {
                //if token is a number, push it to output queue
                if (formula[i].ToLower() == "experience" || formula[i].ToLower() == "exp")
                {
                    outputQueue.Insert(0, lvl.ToString());
                }
                else if (double.TryParse(formula[i], out double n))
                {
                    outputQueue.Insert(0, formula[i]);
                }
                //if token is an operator, push it to operator stack
                else if (formula[i] == "+" || formula[i] == "-")
                {
                    //while there is an operator at top of stack with greater precedence or equal precedence and is left associative, and not a left bracket, pop from operator stack to output queue
                    while (operatorStack.Count != 0 && (operatorStack.Peek() == "^" || operatorStack.Peek() == "root"
                        || operatorStack.Peek() == "+" || operatorStack.Peek() == "-" && operatorStack.Peek() != "("))
                    {
                        outputQueue.Insert(0, operatorStack.Pop());
                    }

                    if (formula[i] == "+")
                    {
                        operatorStack.Push("-");
                    }
                    else
                    {
                        operatorStack.Push("+");
                    }
                    
                }
                else if (formula[i] == "/" || formula[i] == "*")
                {
                    while (operatorStack.Count != 0 && (operatorStack.Peek() == "^" || operatorStack.Peek() == "/" || operatorStack.Peek() == "root" || operatorStack.Peek() == "*"
                        && operatorStack.Peek() != "("))
                    {
                        outputQueue.Insert(0, operatorStack.Pop());
                    }

                    if (formula[i - 1].ToLower() == "exp" || formula[i - 1].ToLower() == "experience" || formula[i + 1].ToLower() == "exp" || formula[i + 1].ToLower() == "experience")
                    {
                        //multDivQueue.Insert(multDivQueue.Count, formula[i]);
                        //multDivQueue.Insert(multDivQueue.Count, formula[i + 1]);
                        //i++;

                        operatorStack.Push(formula[i]);
                    }
                    else
                    {
                        operatorStack.Push(formula[i]);
                    }
                }
                else if (formula[i] == "root" || formula[i] == "^")
                {
                    while (operatorStack.Count != 0 && (operatorStack.Peek() != "("))
                    {
                        outputQueue.Insert(0, operatorStack.Pop());
                    }
                    operatorStack.Push(formula[i]);
                }
                //if token is left bracket, push to operator stack
                else if (formula[i] == "(")
                {
                    operatorStack.Push(formula[i]);
                }
                //if token is right bracket...
                else if (formula[i] == ")")
                {
                    //while operator at top of sack is not left bracket, pop operator from operator stack to output queue
                    while (operatorStack.Count != 0 && operatorStack.Peek() != "(")
                    {
                        outputQueue.Insert(0, operatorStack.Pop());
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
                    throw new System.InvalidOperationException($"You cannot input '{formula[i]}' to the formula. Please check formula input.");
                }
            }
            //if operator on top of stack is bracket, mismatched brackets
            if (operatorStack.Peek() == "(" || operatorStack.Peek() == ")")
            {
                throw new System.InvalidOperationException("Mismatched brackets. Please check formula input.");
            }

            //while (multDivQueue.Count > 0)
            //{
            //    for (int i = 0; i < outputQueue.Count; i++)
            //    {
            //        if (double.TryParse(outputQueue.ElementAt(i), out double n))
            //        {
            //            if (multDivQueue.First() == "/")
            //            {
            //                outputQueue.Insert(i, (Convert.ToDouble(outputQueue.ElementAt(i)) * Convert.ToDouble(multDivQueue.ElementAt(1))).ToString());
            //                outputQueue.RemoveAt(i + 1);
            //            }
            //            else
            //            {
            //                outputQueue.Insert(i, (Convert.ToDouble(outputQueue.ElementAt(i)) / Convert.ToDouble(multDivQueue.ElementAt(1))).ToString());
            //                outputQueue.RemoveAt(i + 1);
            //            }
            //        }
            //    }

            //    for (int i = 2; i < multDivQueue.Count; i++)
            //    {
            //        if (double.TryParse(multDivQueue.ElementAt(i), out double n))
            //        {
            //            if (multDivQueue.First() == "/")
            //            {
            //                multDivQueue.Insert(i, (Convert.ToDouble(multDivQueue.ElementAt(i)) * Convert.ToDouble(multDivQueue.ElementAt(1))).ToString());
            //                multDivQueue.RemoveAt(i + 1);
            //            }
            //            else
            //            {
            //                multDivQueue.Insert(i, (Convert.ToDouble(multDivQueue.ElementAt(i)) / Convert.ToDouble(multDivQueue.ElementAt(1))).ToString());
            //                multDivQueue.RemoveAt(i + 1);
            //            }
            //        }
            //    }

            //    multDivQueue.RemoveAt(0);
            //    multDivQueue.RemoveAt(0);
            //}

            //if there are stil operators on stack, pop from operator stack onto output queue
            while (operatorStack.Count != 0)
            {
                outputQueue.Insert(0, operatorStack.Pop());
            }

            Queue<string> rpnQueue = new Queue<string>();
            while (outputQueue.Count > 0)
            {
                rpnQueue.Enqueue(outputQueue[outputQueue.Count - 1]);
                outputQueue.RemoveAt(outputQueue.Count - 1);
            }

            return RPNCalculator(rpnQueue);
        }
    }
}
