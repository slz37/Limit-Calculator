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
        public static double Convert2Postfix(string infixExp)
        {
            /*
             * Converts a given infix expression to a postfix expression
             * to calculate the values used for testing convergence of
             * limits
             */

            List<String> infixList = new List<string>();
            double ans;
            string postfixExp = "";
            Stack<string> stack = new Stack<string>();
            stack.Push("(");

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
                    stack.Push(token);
                }
                else if (token == ")")
                {
                    /*
                     * Add operators to string until left paren reached,
                     * then pop left paren
                     */
                    for (int i = 0; i <= stack.Count(); i++)
                    {
                        if (stack.Peek() == "(")
                        {
                            stack.Pop();
                            break;
                        }
                        else
                        {
                            postfixExp += " " + stack.Pop();
                        }
                    }
                }
                else if (operators.Any(p => p.Key == token))
                {
                    /*
                     * Add operators in stack >= current operator to string,
                     * then push current operator to stack
                     */
                     for (int i = 0; i <= stack.Count(); i++)
                    {
                        if (stack.Peek() == "(")
                        {
                            break;
                        }
                        else if (operators[stack.Peek()] >= operators[token])
                        {
                            postfixExp += " " + stack.Pop();
                        }
                    }
                    stack.Push(token);
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

            ans = EvaluatePostFix(postfixExp);
            return ans;
        }

        public static double EvaluatePostFix(string postfixExp)
        {
            /*
             * Takes a given postfix expression and evaluates it
             * and returns the valueto the main program to test
             * for convergence
             */

            string x, y, ans;
            string[] postfixList = postfixExp.Split(null);
            Stack<string> stack = new Stack<string>();

            //Operator dictionary to define order of operations
            Dictionary<string, int> operators = new Dictionary<string, int>();
            List<string> singleVarFuncs = new List<string>();
            OperatorFunctions.Operators(operators);
            OperatorFunctions.SingleVariableFuncs(singleVarFuncs);

            //Iterate over all tokens in postfix expression
            foreach (string token in postfixList)
            {
                if ((operators.Any(p => p.Key == token)) & !singleVarFuncs.Any(token.Contains))
                {
                    //Evaluate functions that take two arguments
                    y = stack.Pop();
                    x = stack.Pop();

                    ans = OperatorFunctions.EvaluateExp(x, y, token).ToString();

                    stack.Push(ans);
                }
                else if (singleVarFuncs.Any(token.Contains))
                {
                    //Evaluate functions that only take a single argument
                    x = "temp";
                    y = stack.Pop();

                    ans = OperatorFunctions.EvaluateExp(x, y, token).ToString();

                    stack.Push(ans);
                }
                else
                {
                    stack.Push(token);
                }
            }

            //Last thing on stack is the answer
            ans = stack.Pop();
            return double.Parse(ans);
        }

        public static double Calculate(string func, double value)
        {
            /*
             * Takes a function replaces variables with value, converts to postfix,
             * and evaluates the function
             */

            double ans;

            //Replace and fill in any constant values
            string analytic_func = StringFunctions.ReplaceConstants(func, value);

            ans = Convert2Postfix(analytic_func);
            return ans;
        }
    }
}
