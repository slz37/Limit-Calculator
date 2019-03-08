﻿using System;
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
        public static bool IsComplete(string func)
        {
            //First reverse expression back to normal postfix
            string funcNormal = StringFunctions.ReverseString(func);
            string funcEval = StringFunctions.ReplaceConstants(funcNormal, Math.Tan(2 + Math.Exp(10)) + Math.PI);

            //Simplest way is to try to evaluate it and see what happens
            try
            {
                /*
                 * Pick a value that's extremely unlikely to cause
                 * any issues other than if the expression is complete
                 * or not.
                 */
                Calculator.EvaluatePostFix(funcEval);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Takes in a string that contains an expression and returns
        /// a boolean value if the expression is singular, i.e. it
        /// does not contain any operators.
        /// </summary>
        /// <param name="func"></param>
        /// <returns></returns>
        public static bool IsSingular(string[] func)
        {
            //Operator lists for different number of operands
            Dictionary<string, int> operators = new Dictionary<string, int>();
            OperatorFunctions.Operators(operators);

            //Skip placeholder strings
            if (func.Contains("temp"))
            {
                return false;
            }

            //Check all tokens for operators
            foreach (string token in func)
            {
                if (operators.Any(p => p.Key == token))
                {
                    return false;
                }
            }

            return true;
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

            //Now pass to recursive loop
            funcDer = CompleteExpressions(postfixArray);
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

            bool completeA = IsComplete(A);
            bool completeB = IsComplete(B);
            
            //Loop until complete expressons
            while ((!completeA) || (!completeB))
            {
                //Try to fix completeness
                if (!completeB)
                {
                    B += " " + A;
                    A = postfixArray[i + 3];
                }
                else if (!completeA)
                {
                    A += " " + postfixArray[i + 3];
                }

                //Recheck for completeness and increment
                completeA = IsComplete(A);
                completeB = IsComplete(B);
                i++;
            }

            //Reverse strings back to normal and take derivative
            //A = StringFunctions.ReverseString(A);
            //B = StringFunctions.ReverseString(B);
            string[] ArrA = A.Split(' ');
            string[] ArrB = B.Split(' ');
            return EvaluateDerivative(ArrA, ArrB, token);
        }

        /// <summary>
        /// Takes a given expression and evaluates the
        /// derivative of it.
        /// </summary>
        /// <param name="func"></param>
        /// <returns></returns>
        private static string EvaluateDerivative(string[] A, string[] B, string token)
        {
            string[] tempList = {"temp"};

            //Derivatives of constants
            if (IsSingular(A))
            {
                return "0";
            }
            //Derivates of x
            else if (IsSingular(B))
            {
                return "1";
            }
            else if ((token == "+") || (token == "-"))
            {
                if (IsSingular(A))
                {
                    //Singular expression - check whether x or number
                    if (int.TryParse(A[0], out int n))
                    {
                        string tempA = EvaluateDerivative(A, tempList, "temp");
                    }
                    else
                    {
                        string tempA = EvaluateDerivative(tempList, A, "temp");
                    }
                }
                else if (IsSingular(B))
                {
                    //Singular expression - check whether x or number
                    if (int.TryParse(B[0], out int n))
                    {
                        string tempB = EvaluateDerivative(A, tempList, "temp");
                    }
                    else
                    {
                        string tempB = EvaluateDerivative(tempList, A, "temp");
                    }
                }
                else
                {
                    return CompleteExpressions(A) + token + CompleteExpressions(B);
                }
                return CompleteExpressions(A);
            }
            else if (token == "cos")
            {
                return EvaluateDerivative(A, B, "const") + " " + "*" + " " + A + " " + "sin";
            }
            /*
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
            */
            else
            {
                return A[0];
            }
        }
    }
}