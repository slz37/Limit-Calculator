using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Limit_Calculator
{
    class Calculator
    {
        /// <summary>
        /// Converts a given postfix array to a infix expression.
        /// </summary>
        /// <param name="postfixExp"></param>
        /// <returns></returns>
        public static string Convert2Infix(string[] postfixExp)
        {
            Stack<string> tokenStack = new Stack<string>();
            List<string> postfixList = new List<string>();

            //Operator lists for different number of operands
            List<string> unaryOperators = new List<string>();
            List<string> binaryOperators = new List<string>();
            OperatorFunctions.UnaryOperators(unaryOperators);
            OperatorFunctions.BinaryOperators(binaryOperators);

            //Recreate postfix list
            for (int i = 0; i < postfixExp.Length; i++)
            {
                postfixList.Add(postfixExp[i]);
            }
            postfixList.Reverse();

            //Iterate over all tokens in postfix expression
            foreach (string token in postfixList)
            {
                //Binary operators
                if (binaryOperators.Any(token.Contains))
                {
                    string x = tokenStack.Pop();
                    string y = tokenStack.Pop();
                    string exp = "(" + y + token + x + ")";

                    tokenStack.Push(exp);
                }
                //Unary operators
                else if (unaryOperators.Any(token.Contains))
                {
                    string x = tokenStack.Pop();
                    string exp = "(" + token + "(" + x + ")" + ")";

                    tokenStack.Push(exp);
                }
                /*
                * Add Numbers directly to string, the first
                * one doesn't need a buffer space in-between
                */
                else
                {
                    tokenStack.Push(token);
                }
            }

            return tokenStack.Pop();
        }

        /// <summary>
        /// Converts a given infix expression to a postfix expression
        /// to calculate the values used for testing convergence of
        /// limits.
        /// </summary>
        /// <param name="infixExp"></param>
        /// <returns></returns>
        public static string Convert2Postfix(string infixExp)
        {
            List<string> infixList = new List<string>();
            string postfixExp = "";
            Stack<string> tokenStack = new Stack<string>();
            tokenStack.Push("(");

            //Operator dictionary to define order of operations
            Dictionary<string, int> operators = new Dictionary<string, int>();
            OperatorFunctions.Operators(operators);

            //Convert infix expression to an array of strings
            infixList = StringFunctions.Convert2List(infixExp);
            infixList = StringFunctions.ReplaceNegatives(infixList);
            infixList.Add(")");

            //Iterate over all tokens in infix expression
            foreach (string token in infixList)
            {
                if (token == "(")
                {
                    tokenStack.Push(token);
                }
                else if (token == ")")
                {
                    /*
                     * Add operators to string until left paren reached,
                     * then pop left paren
                     */
                    for (int i = 0; i <= tokenStack.Count(); i++)
                    {
                        if (tokenStack.Peek() == "(")
                        {
                            tokenStack.Pop();
                            break;
                        }
                        else
                        {
                            postfixExp += " " + tokenStack.Pop();
                        }
                    }
                }
                else if (operators.Any(p => p.Key == token))
                {
                    /*
                     * Add operators in stack >= current operator to string,
                     * then push current operator to stack
                     */
                     for (int i = 0; i <= tokenStack.Count(); i++)
                    {
                        if (tokenStack.Peek() == "(")
                        {
                            break;
                        }
                        else if (operators[tokenStack.Peek()] >= operators[token])
                        {
                            postfixExp += " " + tokenStack.Pop();
                        }
                    }
                    tokenStack.Push(token);
                }
                else
                {
                    /*
                     * Add Numbers directly to string, the first
                     * one doesn't need a buffer space in-between
                     */
                    if (postfixExp.Length == 0)
                    {
                        postfixExp += token;
                    }
                    else
                    {
                        postfixExp += " " + token;
                    }
                }
            }

            return postfixExp;
        }

        /// <summary>
        /// Takes a given postfix expression and evaluates it
        /// and returns the value to the main program to test
        /// for convergence.
        /// </summary>
        /// <param name="postfixExp"></param>
        /// <returns></returns>
        public static double EvaluatePostFix(string postfixExp)
        {
            string x, y, ans;
            string[] postfixArray = postfixExp.Split(null);
            Stack<string> tokenStack = new Stack<string>();

            //Operator lists for different number of operands
            List<string> unaryOperators = new List<string>();
            List<string> binaryOperators = new List<string>();
            OperatorFunctions.UnaryOperators(unaryOperators);
            OperatorFunctions.BinaryOperators(binaryOperators);

            //Iterate over all tokens in postfix expression
            foreach (string token in postfixArray)
            {
                if (binaryOperators.Any(token.Contains))
                {
                    //Evaluate functions that take two arguments
                    y = tokenStack.Pop();
                    x = tokenStack.Pop();

                    ans = OperatorFunctions.EvaluateExp(x, y, token).ToString();

                    tokenStack.Push(ans);
                }
                else if (unaryOperators.Any(token.Contains))
                {
                    //Evaluate functions that only take a single argument
                    x = "temp";
                    y = tokenStack.Pop();

                    ans = OperatorFunctions.EvaluateExp(x, y, token).ToString();

                    tokenStack.Push(ans);
                }
                else
                {
                    tokenStack.Push(token);
                }
            }

            //Last thing on stack is the answer
            ans = tokenStack.Pop();
            return double.Parse(ans);
        }

        /// <summary>
        /// Takes a function replaces variables with value, converts to postfix,
        /// and evaluates the function.
        /// </summary>
        /// <param name="func"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static double Calculate(string func, double value)
        {
            double ans;
            string postFixExp;

            //Replace and fill in any constant values
            string analytic_func = StringFunctions.ReplaceConstants(func, value);

            //Convert to postfix and then evaluate
            postFixExp = Convert2Postfix(analytic_func);
            ans = EvaluatePostFix(postFixExp);

            return ans;
        }
    }
}
