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
            string[] postfixArray = func.Split(null);
            string funcDer;

            //Reverse array to do derivatives outside -> in
            Array.Reverse(postfixArray);

            //Now pass to recursive loop
            funcDer = CompleteExpressions(postfixArray);

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
            //Operator lists for different number of operands
            List<string> unaryOperators = new List<string>();
            List<string> binaryOperators = new List<string>();
            OperatorFunctions.UnaryOperators(unaryOperators);
            OperatorFunctions.BinaryOperators(binaryOperators);

            //Initialize
            int i = 0;
            string token = postfixArray[0];

            //Split between no, unary, and binary operators
            if (unaryOperators.Any(token.Contains))
            {
                string A = postfixArray[1];
                string B = "temp";

                bool completeA = IsComplete(A);

                //Loop until complete expressons
                while (!completeA)
                {
                    i++;

                    if (!completeA)
                    {
                        A += " " + postfixArray[i + 1];
                    }

                    //Recheck for completeness and increment
                    completeA = IsComplete(A);
                }

                string[] ArrA = A.Split(' ');
                string[] ArrB = B.Split(' ');
                return EvaluateDerivative(ArrA, ArrB, token);
            }
            else if (binaryOperators.Any(token.Contains))
            {
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

                string[] ArrA = A.Split(' ');
                string[] ArrB = B.Split(' ');
                return EvaluateDerivative(ArrA, ArrB, token);
            }
            else
            {
                string[] tempList = {"temp"};
                return EvaluateDerivative(tempList, postfixArray, "temp");
            }
        }

        /// <summary>
        /// Takes a given expression and evaluates the
        /// derivative of it. Acts as a dictionary of
        /// derivatives of operators.
        /// </summary>
        /// <param name="func"></param>
        /// <returns></returns>
        private static string EvaluateDerivative(string[] A, string[] B, string token)
        {
            string[] tempList = {"temp"};
            string tempA = "", tempB = "";

            if ((IsSingular(A)) & (token != "temp"))
            {
                //Singular expression - check whether x or number
                if (float.TryParse(A[0], out float j))
                {
                    tempA = EvaluateDerivative(A, tempList, "temp");
                }
                else
                {
                    tempA = EvaluateDerivative(tempList, A, "temp");
                }
            }
            if ((IsSingular(B)) &(token != "temp"))
            {
                //Singular expression - check whether x or number
                if (float.TryParse(B[0], out float k))
                {
                    tempB = EvaluateDerivative(B, tempList, "temp");
                }
                else
                {
                    tempB = EvaluateDerivative(tempList, B, "temp");
                }
            }

            //Derivatives of constants
            if ((IsSingular(A)) & (token == "temp"))
            {
                return "0";
            }
            //Derivates of x
            else if ((IsSingular(B)) & (token == "temp"))
            {
                return "1";
            }
            /* Relic - might not be needed
            else if ((token == "~") & (B[0] == "temp"))
            {
                    return "(" + "-" + CompleteExpressions(A);
            }
            */
            else if ((token == "+") || (token == "-"))
            {
                //Call derivatives again if not done
                if (!String.IsNullOrEmpty(tempA))
                {
                    if (!String.IsNullOrEmpty(tempB))
                    {
                        return "(" + tempA + token + tempB + ")";
                    }
                    else
                    {
                        return "(" + tempA + token + CompleteExpressions(B) + ")";
                    }
                }
                else if (!String.IsNullOrEmpty(tempB))
                {
                    return "(" + CompleteExpressions(A) + token + tempB + ")";
                }
                else
                {
                    return "(" + CompleteExpressions(A) + token + CompleteExpressions(B) + ")";
                }
            }
            else if (token == "*")
            {
                //Call derivatives again if not done
                if (!String.IsNullOrEmpty(tempA))
                {
                    if (!String.IsNullOrEmpty(tempB))
                    {
                        return "(" + tempB + token + A[0] + "+" + tempA + token + B[0] + ")";
                    }
                    else
                    {
                        //Convert to infix
                        string stringB = Calculator.Convert2Infix(B);
                        return "(" + CompleteExpressions(B) + token + A[0] + "+" + tempA + token + stringB + ")";
                    }
                }
                else if (!String.IsNullOrEmpty(tempB))
                {
                    //Convert to infix
                    string stringA = Calculator.Convert2Infix(A);
                    return "(" + tempB + token + stringA + "+" + B[0] + token + CompleteExpressions(A) + ")";
                }
                else
                {
                    //Convert to infix
                    string stringA = Calculator.Convert2Infix(A);
                    string stringB = Calculator.Convert2Infix(B);
                    return "(" + stringB + token + CompleteExpressions(A) + "+" + stringA + token + CompleteExpressions(B) + ")";
                }
            }
            else if (token == "/")
            {
                //Call derivatives again if not done
                if (!String.IsNullOrEmpty(tempA))
                {
                    if (!String.IsNullOrEmpty(tempB))
                    {
                        return "(" + "(" + B[0] + "*" + tempA + "-" + A[0] + "*" + tempB + ")" + token + "(" + B[0] + "*" + B[0] + ")" + ")";
                    }
                    else
                    {
                        //Convert to infix
                        string stringB = Calculator.Convert2Infix(B);
                        return "(" + "(" + stringB + "*" + tempA + "-" + A[0] + "*" + CompleteExpressions(B) +
                                ")" + token + "(" + stringB + "*" + stringB + ")" + ")";
                    }
                }
                else if (!String.IsNullOrEmpty(tempB))
                {
                    //Convert to infix
                    string stringA = Calculator.Convert2Infix(A);
                    return "(" + "(" + B[0] + "*" + CompleteExpressions(A) + "-" + stringA + "*" + tempB + ")" +
                            token + "(" + B[0] + "*" + B[0] + ")" + ")";
                }
                else
                {
                    //Convert to infix
                    string stringA = Calculator.Convert2Infix(A);
                    string stringB = Calculator.Convert2Infix(B);
                    return "(" + "(" + stringB + "*" + CompleteExpressions(A) + "-" + stringA + "*" + CompleteExpressions(B) + ")" +
                            token + "(" + stringB + "*" + stringB + ")" + ")";
                }
            }
            else if (token == "ln")
            {
                //ln(a)
                if (tempA == "0")
                {
                    return "(" + "0" + ")";
                }
                //ln(x)
                else if (tempA == "1")
                {
                    return "(" + "1" + "/" + A[0] + ")";
                }
                //ln(x...)
                else
                {
                    //Convert to infix
                    string stringA = Calculator.Convert2Infix(A);

                    return "(" + CompleteExpressions(A) + "*" + "1" + "/" + stringA + ")";
                }
            }
            //x^2 -> A=x B=2
            //2^x -> B=2 A=x
            else if (token == "^")
            {
                //Call derivatives again if not done
                if (tempA == "0")
                {
                    //a^a
                    if (tempB == "0")
                    {
                        return "(" + "0" + ")";
                    }
                    //a^x
                    else
                    {
                        //Convert to infix
                        string stringB = Calculator.Convert2Infix(B);

                        //Create array of ln(a^x) to take derivative of
                        List<string> lnBList = new List<string> { "*", "ln", A[0] };
                        foreach (string element in B)
                        {
                            lnBList.Add(element);
                        }
                        string[] lnB = lnBList.ToArray();

                        //d/dx = a^x * d/dx (ln(a^x))
                        return "(" + A[0] + token + stringB + "*" + CompleteExpressions(lnB) + ")";
                    }
                }
                //x^a
                else if (tempB == "0")
                {
                    //Convert to infix
                    string stringA = Calculator.Convert2Infix(A);

                    //Create array of ln(x^a) to take derivative of
                    List<string> lnAList = new List<string> { "*", "ln" };
                    foreach (string element in A)
                    {
                        lnAList.Add(element);
                    }
                    lnAList.Add(B[0]);
                    string[] lnA = lnAList.ToArray();

                    return "(" + stringA + token + B[0] + "*" + CompleteExpressions(lnA) + ")";
                }
                //x^x
                else
                {
                    //Convert to infix
                    string stringA = Calculator.Convert2Infix(A);
                    string stringB = Calculator.Convert2Infix(B);

                    //Create array of ln(x^x) to take derivative of
                    List<string> lnAList = new List<string> { "*", "ln" };
                    foreach (string element in A)
                    {
                        lnAList.Add(element);
                    }
                    foreach (string element in B)
                    {
                        lnAList.Add(element);
                    }
                    string[] lnAB = lnAList.ToArray();

                    return "(" + stringA + token + stringB + "*" + CompleteExpressions(lnAB) + ")";
                }
            }
            else if (token  == "cos")
            {
                //cos(a)
                if (tempA == "0")
                {
                    return "(" + "0" + ")";
                }
                //cos(x)
                else if (tempA == "1")
                {
                    return "(" + "-" +  "sin" + "(" + A[0] + ")" +  ")";
                }
                //cos(x...)
                else
                {
                    //Convert to infix
                    string stringA = Calculator.Convert2Infix(A);

                    return "(" + CompleteExpressions(A) + "*" + "(" + "-" + "sin" + "(" + stringA + ")" + ")" + ")";
                }
            }
            else if (token == "sin")
            {
                //sin(a)
                if (tempA == "0")
                {
                    return "(" + "0" + ")";
                }
                //sin(x)
                else if (tempA == "1")
                {
                    return "(" + "cos" + "(" + A[0] + ")" + ")";
                }
                //sin(x...)
                else
                {
                    //Convert to infix
                    string stringA = Calculator.Convert2Infix(A);

                    return "(" + CompleteExpressions(A) + "*" + "(" + "cos" + "(" + stringA + ")" + ")" + ")";
                }
            }
            else if (token == "tan")
            {
                //tan(a)
                if (tempA == "0")
                {
                    return "(" + "0" + ")";
                }
                //tan(x)
                else if (tempA == "1")
                {
                    return "(" + "sec" + "(" + A[0] + ")" + "^2" + ")";
                }
                //tan(x...)
                else
                {
                    //Convert to infix
                    string stringA = Calculator.Convert2Infix(A);

                    return "(" + CompleteExpressions(A) + "*" + "(" + "sec" + "(" + stringA + ")" + "^2" + ")" + ")";
                }
            }
            else if (token == "arccos")
            {
                //arccos(a)
                if (tempA == "0")
                {
                    return "(" + "0" + ")";
                }
                //arccos(x)
                else if (tempA == "1")
                {
                    return "(" + "-" + "(" + "1" + "/" + "sqrt" + "(" + "1" + "-" + A[0] + "^2" + ")" + ")" + ")";
                }
                //arccos(x...)
                else
                {
                    //Convert to infix
                    string stringA = Calculator.Convert2Infix(A);

                    return "(" + CompleteExpressions(A) + "*" + "(" + "-" + "(" + "1" + "/" + "sqrt" + "(" + "1" + "-" + stringA + "^2" + ")" + ")" + ")" + ")";
                }
            }
            else if (token == "arcsin")
            {
                //arcsin(a)
                if (tempA == "0")
                {
                    return "(" + "0" + ")";
                }
                //arcsin(x)
                else if (tempA == "1")
                {
                    return "(" + "1" + "/" + "sqrt" + "(" + "1" + "-" + A[0] + "^2" + ")" + ")";
                }
                //arcsin(x...)
                else
                {
                    //Convert to infix
                    string stringA = Calculator.Convert2Infix(A);

                    return "(" + CompleteExpressions(A) + "*" + "(" + "1" + "/" + "sqrt" + "(" + "1" + "-" + stringA + "^2" + ")" + ")" + ")";
                }
            }
            else if (token == "arctan")
            {
                //arctan(a)
                if (tempA == "0")
                {
                    return "(" + "0" + ")";
                }
                //arctan(x)
                else if (tempA == "1")
                {
                    return "(" + "1" + "/" + "(" + "1" + "+" + A[0] + "^2" + ")" + ")";
                }
                //arctan(x...)
                else
                {
                    //Convert to infix
                    string stringA = Calculator.Convert2Infix(A);

                    return "(" + CompleteExpressions(A) + "*" + "(" + "1" + "/" + "(" + "1" + "+" + stringA + "^2" + ")" + ")" + ")";
                }
            }
            else if (token == "cot")
            {
                //cot(a)
                if (tempA == "0")
                {
                    return "(" + "0" + ")";
                }
                //cot(x)
                else if (tempA == "1")
                {
                    return "(" + "-" + "(" + "csc" + "(" + A[0] + ")" + "^2" + ")" + ")";
                }
                //cot(x...)
                else
                {
                    //Convert to infix
                    string stringA = Calculator.Convert2Infix(A);

                    return "(" + CompleteExpressions(A) + "*" + "(" + "-" + "(" + "csc" + "(" + stringA + ")" + "^2" + ")" + ")" + ")";
                }
            }
            else if (token == "sec")
            {
                //sec(a)
                if (tempA == "0")
                {
                    return "(" + "0" + ")";
                }
                //sec(x)
                else if (tempA == "1")
                {
                    return "(" + "tan" + "(" + A[0] + ")" + "*" + "sec" + "(" + A[0] + ")" + ")";
                }
                //sec(x...)
                else
                {
                    //Convert to infix
                    string stringA = Calculator.Convert2Infix(A);

                    return "(" + CompleteExpressions(A) + "*" + "(" + "tan" + "(" + stringA + ")" + "*" + "sec" + "(" + stringA + ")" + ")" + ")";
                }
            }
            else if (token == "csc")
            {
                //csc(a)
                if (tempA == "0")
                {
                    return "(" + "0" + ")";
                }
                //csc(x)
                else if (tempA == "1")
                {
                    return "(" + "-" + "cot" + "(" + A[0] + ")" + "*" + "csc" + "(" + A[0] + ")" + ")";
                }
                //csc(x...)
                else
                {
                    //Convert to infix
                    string stringA = Calculator.Convert2Infix(A);

                    return "(" + CompleteExpressions(A) + "*" + "(" + "-" + "cot" + "(" + stringA + ")" + "*" + "csc" + "(" + stringA + ")" + ")" + ")";
                }
            }
            else if (token == "sqrt")
            {
                //sqrt(a)
                if (tempA == "0")
                {
                    return "(" + "0" + ")";
                }
                //sqrt(x)
                else if (tempA == "1")
                {
                    return "(" + "0.5" + "*" + "(" + "1" + "/" + "sqrt" + "(" + A[0] + ")" + ")" + ")";
                }
                //sqrt(x...)
                else
                {
                    //Convert to infix
                    string stringA = Calculator.Convert2Infix(A);

                    return "(" + "0.5" + "*" + CompleteExpressions(A) + "*" + "(" + "1" + "/" + "sqrt" + "(" + stringA + ")" + ")" + ")";
                }
            }
            else if (token == "abs")
            {
                //abs(a)
                if (tempA == "0")
                {
                    return "(" + "0" + ")";
                }
                //abs(x)
                else if (tempA == "1")
                {
                    return "(" + A[0] + "/" + "abs" + "(" + A[0] + ")" + ")";
                }
                //abs(x...)
                else
                {
                    //Convert to infix
                    string stringA = Calculator.Convert2Infix(A);

                    return "(" + CompleteExpressions(A) + "*" + "(" + stringA + "/" + "abs" + "(" + stringA + ")" + ")" + ")";
                }
            }
            else if (token == "log")
            {
                if (tempA == "0")
                {
                    //log_a(a)
                    if (tempB == "0")
                    {
                        return "(" + "0" + ")";
                    }
                    //log_a(x)
                    else if (tempB == "1")
                    {
                        return "(" + "1" + "/" + "(" + B[0] + "*" + "ln" + "(" + A[0] + ")" + ")" + ")";
                    }
                    //log_a(x...)
                    else
                    {
                        //Convert to infix
                        string stringB = Calculator.Convert2Infix(B);

                        return "(" + CompleteExpressions(B) + "/" + "(" + stringB + "*" + "ln" + "(" + A[0] + ")" + ")" + ")";
                        //return "(" + "-" + "ln" + "(" + A[0] + ")" + "/" + "(" + B[0] + "*" + "ln" + "(" + B[0] + ")" + "^" + "2" + ")" + ")";
                    }
                }
                else if (tempB == "0")
                {
                    //log_a(x)
                    return "(" + "1" + "/" + "(" + A[0] + "*" + "ln" + "(" + B[0] + ")" + ")" + ")";
                }
                //log(x...)
                else
                {
                    return "";
                }
            }
            else
            {
                //Error calculating derivative
                throw new System.ArgumentException("Error in evaluating derivative!", "original");
            }
        }
    }
}
