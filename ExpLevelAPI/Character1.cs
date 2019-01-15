using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpLevelAPI
{
    class Character1
    {
        private List<string> formula;
        public Character1(List<string> form)
        {
            formula = form;
        }
        public long FormulaCalc()
        {
            //Shunting-yard algorithm
            Queue<string> outputQueue = new Queue<string>();
            Stack<string> operatorStack = new Stack<string>();
            Stack<long> rpnStack = new Stack<long>(); //reverse polish notation stack, used for end calculation

            foreach (string i in formula)
            {
                if (long.TryParse(i, out long n))
                {
                    outputQueue.Enqueue(i);
                }
                else if (i == "+" || i == "-")
                {
                    while (operatorStack.Count != 0 && (operatorStack.Peek() == "/" || operatorStack.Peek() == "*" || operatorStack.Peek() == "^" || operatorStack.Peek() == "+"
                        || operatorStack.Peek() == "-" && operatorStack.Peek() != "("))
                    {
                        outputQueue.Enqueue(operatorStack.Pop());
                    }

                    operatorStack.Push(i);
                }
                else if (i == "/" || i == "*")
                {
                    while(operatorStack.Count != 0 && (operatorStack.Peek() == "^" || operatorStack.Peek() == "/" || operatorStack.Peek() == "*" && operatorStack.Peek() != "("))
                    {
                        outputQueue.Enqueue(operatorStack.Pop());
                    }
                    
                    operatorStack.Push(i);
                }
                else if (i == "^")
                {
                    while (operatorStack.Count != 0 && (operatorStack.Peek() != "("))
                    {
                        outputQueue.Enqueue(operatorStack.Pop());
                    }

                    operatorStack.Push(i);
                }
                else if (i == "(")
                {
                    operatorStack.Push(i);
                }
                else if (i == ")")
                {
                    //TODO:: TEST WHEN UNEQUAL AMOUNT OF BRACKETS
                    while (operatorStack.Count != 0 && operatorStack.Peek() != "(")
                    {
                        outputQueue.Enqueue(operatorStack.Pop());
                    }

                    operatorStack.Pop();
                }
            }
            while (operatorStack.Count != 0)
            {
                outputQueue.Enqueue(operatorStack.Pop());
            }

            foreach(string i in outputQueue)
            {
                long popNum = 0;
                if(long.TryParse(i, out long n))
                {
                    rpnStack.Push(Convert.ToInt64(i));
                }
                else if(i == "+")
                {
                    rpnStack.Push(rpnStack.Pop() + rpnStack.Pop());
                }
                else if(i == "-")
                {
                    popNum = rpnStack.Pop();
                    rpnStack.Push(rpnStack.Pop() - popNum);
                }
                else if(i == "*")
                {
                    popNum = rpnStack.Pop();
                    rpnStack.Push(rpnStack.Pop() * popNum);
                }
                else if(i == "/")
                {
                    popNum = rpnStack.Pop();
                    rpnStack.Push(rpnStack.Pop() / popNum);
                }
                else if(i == "^")
                {
                    popNum = rpnStack.Pop();
                    rpnStack.Push((long)Math.Pow(rpnStack.Pop(), popNum));
                }
            }

            return rpnStack.Pop();
        }
    }
}
