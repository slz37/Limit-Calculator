using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Limit_Calculator
{
    class StringFunctions
    {
        public static string ReplaceConstants(string Exp, double value = 0)
        {
            /*
             * Takes in a string expression and replaces with the specified
             * value. Also replaces common constants such as pi, phi, e, etc.
             * to arbitrary precision.
             */

            Exp = Exp.Replace("x", value.ToString());
            Exp = Exp.Replace("pi", "3.14159");
            Exp = Exp.Replace("e", "2.71828");
            Exp = Exp.Replace("phi", "1.61703");
            return Exp;
        }
        public static List<string> ReplaceNegatives(List<string> Exp)
        {
            /*
             * This function takes an analytic expression and determines
             * whether a "-" is a subtraction of two values or a negation
             * of one value. Replaces all negations with "~" instead of "-".
             */

            //Deep clone list for constant length to iterate over
            List<string> ExpOut = new List<string>();
            ExpOut = DeepCloneList(Exp);
            int StrLen = Exp.Count();

            //Operator dictionary
            Dictionary<string, int> operators = new Dictionary<string, int>();
            OperatorFunctions.Operators(operators);

            for (int i = 0; i < StrLen - 1; i++)
            {
                if ((operators.Any(p => p.Key == ExpOut[i])) & (ExpOut[i + 1] == "-"))
                {
                    //Case a + -b
                    ExpOut[i + 1] = "~";
                }
                if ((ExpOut[i] == "(") & (ExpOut[i + 1] == "-"))
                {
                    //Case (-a)
                    ExpOut[i + 1] = "~";
                }
                if ((i == 0) & (ExpOut[i] == "-"))
                {
                    //Case -a + b
                    ExpOut[i] = "~";
                }
                if ((ExpOut[i] == "-") & (ExpOut[i + 1] == "("))
                {
                    //Case -(a + b)
                    ExpOut[i] = "~";
                }
                if (((ExpOut[i] == "-") & (ExpOut[i + 1] == "~")) |
                    ((ExpOut[i] == "~") & (ExpOut[i + 1] == "-")))
                {
                    //Case !-a or -!a
                    ExpOut.RemoveRange(i, 2);
                }
                if (((ExpOut[i] == "~") & (ExpOut[i + 1] == "~")) |
                    ((ExpOut[i] == "-") & (ExpOut[i + 1] == "-")))
                {
                    //Case !!a or --a
                    ExpOut.RemoveRange(i, 2);
                }
            }

            return ExpOut;
        }

        public static List<string> Convert2List(string Exp)
        {
            /*
             * Takes a string containing a function and converts it
             * to a list, where each element is either a number or
             * operator
             */

            List<string> newExp = new List<string>();
            Queue<string> queue = new Queue<string>();
            int curInd = 0;

            //Operator dictionary to define order of operations

            foreach (char token in Exp)
            {
                if ((char.IsDigit(token)) | (token == '.'))
                {
                    //If last digit, clear queue and add to list, else add to queue
                    if (curInd == Exp.Count() - 1)
                    {
                        //Deep clone list for constant length to iterate over
                        Queue<string> tempQueue = new Queue<string>();
                        tempQueue = DeepCloneQueue(queue);
                        int queueSize = tempQueue.Count();

                        //Clear queue or add to list if queue empty
                        if (queueSize != 0)
                        {
                            string tempStr = "";
                            for (int i = 0; i < queueSize; i++)
                            {
                                tempStr += queue.Dequeue();
                            }
                            tempStr += token.ToString();
                            newExp.Add(tempStr);
                        }
                        else
                        {
                            newExp.Add(token.ToString());
                        }
                    }
                    else
                    {
                        //Not last digit, so just add it to the queue
                        queue.Enqueue(token.ToString());
                    }
                }
                else if (char.IsLetter(token))
                {
                    //If token is a letter, push to queue to complete word
                    queue.Enqueue(token.ToString());
                }
                else
                {
                    /*
                     * If token is an operator append everything in queue as single string
                     * and append operator to next index in list
                     */

                    //Deep clone queue for constant length to iterate over
                    Queue<string> tempQueue = new Queue<string>();
                    tempQueue = DeepCloneQueue(queue);
                    int queueSize = tempQueue.Count();

                    //Clear queue and add full number to list, then add operator to list
                    if (queueSize != 0)
                    {
                        string tempStr = "";
                        for (int i = 0; i < queueSize; i++)
                        {
                            tempStr += queue.Dequeue();
                        }
                        newExp.Add(tempStr);
                    }
                    newExp.Add(token.ToString());
                }
                curInd += 1;
            }

            return newExp;
        }

        private static Queue<string> DeepCloneQueue(Queue<string> Exp)
        {
            /* Takes an input queue and creates a deep
             * clone. This allows us to get a constant
             * length variable for iterating over.
             */

            //Temp queue
            Queue<string> newList = new Queue<string>();

            //Clone elements of old queue to new list
            foreach (string element in Exp)
            {
                newList.Enqueue((string)element.Clone());
            }

            return newList;
        }

        private static List<string> DeepCloneList(List<string> Exp)
        {
            /* Takes an input list and creates a deep
             * clone. This allows us to get a constant
             * length variable for iterating over.
             */

            //Temp list
            List<string> newList = new List<string>();

            //Clone elements of old list to new list
            foreach (string element in Exp)
            {
                newList.Add((string)element.Clone());
            }

            return newList;
        }
    }
}
