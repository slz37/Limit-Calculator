using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Limit_Calculator
{
    class DerivativeCalculator
    {
        /// <summary>
        /// Takes in a string that contains an expression and returns
        /// a boolean value if the expression is valid, i.e. it has
        /// an operator and the appropriate number of operands.
        /// </summary>
        /// <param name="func"></param>
        /// <returns></returns>
        private static bool IsComplete(string func)
        {
            //Simplest way is to try to evaluate it and see what happens
            try
            {
                /*
                 * Pick a value that's extremely unlikely to cause
                 * any issues other than if the expression is complete
                 * or not.
                 */
                Calculator.Calculate(func, Math.Tan(2 + Math.Exp(10)) + Math.PI);
                return true;
            }
            catch
            {
                return false;
            }

        }

        /// <summary>
        /// Takes in a given postfix expression and
        /// performs the initial setup necessary to
        /// calculate derivatives.
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public static string Derivative(string func)
        {
            Stack<string> derivativeStack = new Stack<string>();
            Stack<string> tokenStack = new Stack<string>();
            string[] postfixArray = func.Split(null);
            string funcDer = "";

            //Operator lists for different number of operands
            List<string> unaryOperators = new List<string>();
            List<string> binaryOperators = new List<string>();
            Dictionary<string, int> operators = new Dictionary<string, int>();
            OperatorFunctions.UnaryOperators(unaryOperators);
            OperatorFunctions.BinaryOperators(binaryOperators);
            OperatorFunctions.Operators(operators);

            //Reverse array to do derivatives outside -> in
            Array.Reverse(postfixArray);


            /*
            //Loop over all elements of the postfix expression
            for (int i = 0; i < postfixArray.Length; i++)
            {
                string token = postfixArray[i];
                //Derivatives of binary operators
                if (binaryOperators.Any(token.Contains))
                {
                    string B = postfixArray[i + 1];
                    string A = postfixArray[i + 2];

                    //Group tokens until operands satisfy all operators
                    while ((operators.Any(p => p.Key == A)) || (operators.Any(p => p.Key == B)))
                    {
                        if (binaryOperators.Any(B.Contains))
                        {

                        }
                        else if (unaryOperators.Any(B.Contains))
                        {

                        }
                        else if (binaryOperators.Any(A.Contains))
                        {

                        }
                        else if (unaryOperators.Any(A.Contains))
                        {

                        }
                    }

                    string derivative = EvaluateDerivative(A, B, token);
                    derivativeStack.Push(derivative);
                }
                //Derivatives of unary operators
                else if (unaryOperators.Any(token.Contains))
                {
                    string A = "";
                    string B = tokenStack.Pop();

                    string derivative = EvaluateDerivative(A, B, token);
                    derivativeStack.Push(derivative);
                }
                else
                {
                    tokenStack.Push(token);
                }
            }

            //Clear stack now that we're done
            while (derivativeStack.Count > 0)
            {
                funcDer += tokenStack.Pop();
            }
            */

            return funcDer;
        }

        /// <summary>
        /// Takes in a reverse postfix array and recursively grabs
        /// an operator and two operands to be derivated such
        /// that they are all complete expressions.
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        private static string CompleteExpressions(string[] postfixArray)
        {
            //Initialize
            int i = 0;
            string token = postfixArray[0];
            string B = postfixArray[1];
            string A = postfixArray[2];

            //Loop until complete expressons
            while (!IsComplete(A) & !IsComplete(B))
            {
                //Try to fix completeness
                if (!IsComplete(B))
                {
                    B += A;
                    A = postfixArray[i + 3];
                }
                else if (!IsComplete(A))
                {
                    A += postfixArray[i + 3];
                }

                i++;
            }

            //Take derivative now that it's complete
            return EvaluateDerivative(A, B, token);
        }

        /// <summary>
        /// Takes a given expression and evaluates the
        /// derivative of it.
        /// </summary>
        /// <param name="func"></param>
        /// <returns></returns>
        private static string EvaluateDerivative(string A, string B, string token)
        {
            //Derivatives of constants
            if (token == "const")
            {
                return "0";
            }
            //Derivates of x
            else if (token == "x")
            {
                return "1";
            }
            else if ((token == "+") || (token == "-"))
            {
                return EvaluateDerivative(A, "temp", "const") + token + EvaluateDerivative(B, "temp", "const");
            }
            else if (token == "cos")
            {
                return EvaluateDerivative(A, "temp", "const") + " " + "*" + " " + A + " " + "sin";
            }
            else if (token.Contains("^"))
            {
                //Form of n^x
                if (B == "x")
                {
                    //Actually x^x
                    if (A == "x")
                    {
                        //(d/dx) x^x = x^x*(ln(x)+1)
                        return A + token + A + "*" + "(ln(" + A + ")+1";
                    }
                    else
                    {
                        //(d/dx) n^x = n^x * ln(n)
                        return B + token + A + "*" + "ln(" + B + ")";
                    }
                }
                //Form of x^n
                else
                {
                    double digitDec = double.Parse(B) - 1;

                    //(d/dx) x^n = n*x^(n-1)
                    return B + "*" + A + token + "(" + digitDec + ")";
                }
            }
            else
            {
                return A;
            }
        }
    }
}
