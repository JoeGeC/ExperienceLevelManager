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
            Queue<string> outputQueue = new Queue<string>();
            Stack<string> operatorStack = new Stack<string>();

            foreach (string i in formula)
            {
                //if token is a number, push it to output queue
                if (i.ToLower() == "experience" || i.ToLower() == "exp")
                {
                    outputQueue.Enqueue(lvl.ToString());
                }
                else if (double.TryParse(i, out double n))
                {
                    outputQueue.Enqueue(i);
                }
                //if token is an operator, push it to operator stack
                else if (i == "+" || i == "-")
                {
                    //while there is an operator at top of stack with greater precedence or equal precedence and is left associative, and not a left bracket, pop from operator stack to output queue
                    while (operatorStack.Count != 0 && (operatorStack.Peek() == "/" || operatorStack.Peek() == "*" || operatorStack.Peek() == "^" || operatorStack.Peek() == "root"
                        || operatorStack.Peek() == "+" || operatorStack.Peek() == "-" && operatorStack.Peek() != "("))
                    {
                        outputQueue.Enqueue(operatorStack.Pop());
                    }

                    if (i == "+")
                    {
                        operatorStack.Push("-");
                    }
                    else
                    {
                        operatorStack.Push("+");
                    }
                    
                }
                else if (i == "/" || i == "*")
                {
                    while (operatorStack.Count != 0 && (operatorStack.Peek() == "^" || operatorStack.Peek() == "/" || operatorStack.Peek() == "root" || operatorStack.Peek() == "*" 
                        && operatorStack.Peek() != "("))
                    {
                        outputQueue.Enqueue(operatorStack.Pop());
                    }

                    if (i == "/")
                    {
                        operatorStack.Push("*");
                    }
                    else
                    {
                        operatorStack.Push("/");
                    }
                }
                else if (operatorStack.Peek() == "root" || i == "^")
                {
                    while (operatorStack.Count != 0 && (operatorStack.Peek() != "("))
                    {
                        outputQueue.Enqueue(operatorStack.Pop());
                    }

                    if (i == "root")
                    {
                        operatorStack.Push("^");
                    }
                    else
                    {
                        operatorStack.Push("root");
                    }
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
            if (operatorStack.Peek() == "(" || operatorStack.Peek() == ")")
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
    }
}
