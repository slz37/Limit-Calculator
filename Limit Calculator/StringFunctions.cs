using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Limit_Calculator
{
    class StringFunctions
    {
        /// <summary>
        /// Finds the outer most division in a function,
        /// e.g. (x/2+5*x)/(3+6*x) would return
        /// an index of 9.
        /// </summary>
        /// <param name="func"></param>
        /// <returns></returns>
        public static int FindOuterSlash(string func)
        {
            int index = 0;
            Stack<char> tokenStack = new Stack<char>();

            //Iterate over everything in string
            foreach (char token in func)
            {
                if (token == '(')
                {
                    tokenStack.Push(token);
                }
                else if (token == ')')
                {
                    tokenStack.Pop();
                }
                else if ((token == '/') & (tokenStack.Count == 0))
                {
                    break;
                }

                index++;
            }

            return index;
        }

        /// <summary>
        /// Takes in a string expression and replaces with the specified
        /// value. Also replaces common constants such as pi, phi, e, etc.
        /// to arbitrary precision.
        /// </summary>
        /// <param name="Exp"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ReplaceConstants(string Exp, double value = 0, bool replace_x = true)
        {
            //Replace x if desired
            if (replace_x)
            {
                Exp = Exp.Replace("x", value.ToString());
            }
            else { }

            // Replace common math constants
            Exp = Exp.Replace("pi", "3.14159");
            Exp = Exp.Replace("phi", "1.61703");
            Exp = Exp.Replace("inf", (double.PositiveInfinity).ToString());
            Exp = Exp.Replace("-inf", (double.NegativeInfinity).ToString());

            //Careful e in sec and elsewhere
            Exp = Exp.Replace("e", "2.71828");
            Exp = Exp.Replace("s2.71828c", "sec"); //Undo what we just did if sec...
            return Exp;
        }

        /// <summary>
        /// This function takes an analytic expression and determines
        /// whether a "-" is a subtraction of two values or a negation
        /// of one value. Substitutes negations with 0 - value instead.
        /// </summary>
        /// <param name="Exp"></param>
        /// <returns></returns>
        public static List<string> ReplaceNegatives(List<string> Exp)
        {
            //Deep clone list for constant length to iterate over
            List<string> ExpOut = new List<string>();
            //ExpOut = DeepCloneList(Exp);
            ExpOut = Exp;
            int StrLen = Exp.Count();

            //Operator dictionary
            Dictionary<string, int> operators = new Dictionary<string, int>();
            OperatorFunctions.Operators(operators);

            for (int i = 0; i < ExpOut.Count() - 1; i++)
            {
                if ((operators.Any(p => p.Key == ExpOut[i])) & (ExpOut[i + 1] == "-"))
                {
                    //Case a + -b
                    //ExpOut.Insert(i + 1, "0");
                    ExpOut.Insert(i + 1, "(");
                    ExpOut.Insert(i + 2, "0");
                    ExpOut.Insert(i + 5, ")");
                    i += 3;
                }
                else if ((ExpOut[i] == "(") & (ExpOut[i + 1] == "-"))
                {
                    //Case (-a)
                    //ExpOut.Insert(i + 1, "0");
                    ExpOut.Insert(i + 1, "(");
                    ExpOut.Insert(i + 2, "0");
                    ExpOut.Insert(i + 5, ")");
                    i += 3;
                }
                else if ((i == 0) & (ExpOut[i] == "-"))
                {
                    //Case -a + b
                    //ExpOut.Insert(i , "0");
                    ExpOut.Insert(i, "(");
                    ExpOut.Insert(i + 1, "0");
                    ExpOut.Insert(i + 4, ")");
                    i += 2;
                }
                else if ((ExpOut[i] == "-") & (ExpOut[i + 1] == "(") & (i == 0))
                {
                    //Case -(a + b)
                    //ExpOut.Insert(i, "0");
                    ExpOut.Insert(i, "(");
                    ExpOut.Insert(i + 1, "0");
                    ExpOut.Insert(i, "0");
                    i += 2;
                }

                //Relic - might not be needed
                if (((ExpOut[i] == "-") & (ExpOut[i + 1] == "~")) |
                    ((ExpOut[i] == "~") & (ExpOut[i + 1] == "-")))
                {
                    //Case ~-a or -~a
                    ExpOut.RemoveRange(i, 2);
                }
                //Relic - might not be needed
                if (((ExpOut[i] == "~") & (ExpOut[i + 1] == "~")) |
                    ((ExpOut[i] == "-") & (ExpOut[i + 1] == "-")))
                {
                    //Case ~~a or --a
                    ExpOut.RemoveRange(i, 2);
                }

                /*
                    ################
                    ## Old Method ##
                    ################

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
                if ((ExpOut[i] == "-") & (ExpOut[i + 1] == "(") & (i == 0))
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
                } */
            }

            return ExpOut;
        }

        /// <summary>
        /// Takes a string containing a function and converts it
        /// to a list, where each element is either a number or
        /// operator.
        /// </summary>
        /// <param name="Exp"></param>
        /// <returns></returns>
        public static List<string> Convert2List(string Exp)
        {
            List<string> newExp = new List<string>();
            Queue<string> tokenQueue = new Queue<string>();
            int curInd = 0;

            foreach (char token in Exp)
            {
                if ((char.IsDigit(token)) | (token == '.'))
                {
                    //Deep clone list for constant length to iterate over
                    Queue<string> tempQueue = new Queue<string>();
                    tempQueue = DeepCloneQueue(tokenQueue);
                    int queueSize = tempQueue.Count();

                    //If last digit, clear queue and add to list, else add to queue
                    if (curInd == Exp.Count() - 1)
                    {
                        //Clear queue or add to list if queue empty
                        if (queueSize != 0)
                        {
                            string tempStr = "";
                            for (int i = 0; i < queueSize; i++)
                            {
                                tempStr += tokenQueue.Dequeue();
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
                        //If operator in queue clear the first
                        if (queueSize != 0)
                        {
                            if (char.IsLetter(char.Parse(tokenQueue.Peek())))
                            {
                                string tempStr = "";
                                for (int i = 0; i < queueSize; i++)
                                {
                                    tempStr += tokenQueue.Dequeue();
                                }
                                newExp.Add(tempStr);
                            }
                        }

                        //Not last digit, so just add it to the queue
                        tokenQueue.Enqueue(token.ToString());
                    }
                }
                else if (char.IsLetter(token))
                {
                    //If token is a letter, push to queue to complete word
                    tokenQueue.Enqueue(token.ToString());
                }
                else
                {
                    /*
                     * If token is an operator append everything in queue as single string
                     * and append operator to next index in list
                     */

                    //Deep clone queue for constant length to iterate over
                    Queue<string> tempQueue = new Queue<string>();
                    tempQueue = DeepCloneQueue(tokenQueue);
                    int queueSize = tempQueue.Count();

                    //Clear queue and add full number to list, then add operator to list
                    if (queueSize != 0)
                    {
                        string tempStr = "";
                        for (int i = 0; i < queueSize; i++)
                        {
                            tempStr += tokenQueue.Dequeue();
                        }
                        newExp.Add(tempStr);
                    }
                    newExp.Add(token.ToString());
                }
                curInd += 1;
            }

            //If there's any residuals in queue, remove them
            while (tokenQueue.Count > 0)
            {
                newExp.Add(tokenQueue.Dequeue());
            }

            return newExp;
        }

        /// <summary>
        /// Takes an input queue and creates a deep
        /// clone. This allows us to get a constant
        /// length variable for iterating over.
        /// </summary>
        /// <param name="Exp"></param>
        /// <returns></returns>
        private static Queue<string> DeepCloneQueue(Queue<string> Exp)
        {
            //Temp queue
            Queue<string> newQueue = new Queue<string>();

            //Clone elements of old queue to new list
            foreach (string element in Exp)
            {
                newQueue.Enqueue((string)element.Clone());
            }

            return newQueue;
        }

        /// <summary>
        /// Takes an input list and creates a deep
        /// clone. This allows us to get a constant
        /// length variable for iterating over.
        /// </summary>
        /// <param name="Exp"></param>
        /// <returns></returns>
        private static List<string> DeepCloneList(List<string> Exp)
        {
            //Temp list
            List<string> newList = new List<string>();

            //Clone elements of old list to new list
            foreach (string element in Exp)
            {
                newList.Add((string)element.Clone());
            }

            return newList;
        }

        /// <summary>
        /// Takes an input string expression and
        /// reverses it while preserving number and
        /// operator order (e.g. 56, ln, cos are not reversed).
        /// </summary>
        /// <param name="Exp"></param>
        /// <returns></returns>
        public static string ReverseString(string Exp)
        {
            //Convert to array and reverse
            Stack<string> tempStack = new Stack<string>();
            string[] stringArray = Exp.Split(' ');
            string reversedExp = "";

            //Push to stack to reverse
            foreach (string token in stringArray)
            {
                tempStack.Push(token);
            }

            //Recreate reversed string - avoid reversing ln, cos, etc.
            for (int i = 0; i < stringArray.Length; i++)
            {
                if (i != stringArray.Length - 1)
                {
                    reversedExp += tempStack.Pop() + " ";
                }
                else
                {
                    reversedExp += tempStack.Pop();
                }
            }

            return reversedExp;
        }
    }
}
