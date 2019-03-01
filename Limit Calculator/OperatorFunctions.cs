using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Limit_Calculator
{
    class OperatorFunctions
    {
        public static double EvaluateExp(string a, string b, string op)
        {
            /*
             * Evaluate a given operator on values. Converts each
             * input from a string to their equivalent mathematical
             * expression. Doesn't contain every function, but should
             * capture most of them.
             */

            if (op == "+")
            {
                return double.Parse(a) + double.Parse(b);
            }
            else if (op == "-")
            {
                return double.Parse(a) - double.Parse(b);
            }
            else if (op == "*")
            {
                return double.Parse(a) * double.Parse(b);
            }
            else if (op == "/")
            {
                return double.Parse(a) / double.Parse(b);
            }
            else if (op == "%")
            {
                return double.Parse(a) % double.Parse(b);
            }
            else if (op == "cos")
            {
                return Math.Cos(double.Parse(b));
            }
            else if (op == "sin")
            {
                return Math.Sin(double.Parse(b));
            }
            else if (op == "tan")
            {
                return Math.Tan(double.Parse(b));
            }
            else if (op == "arccos")
            {
                return Math.Acos(double.Parse(b));
            }
            else if (op == "arcsin")
            {
                return Math.Asin(double.Parse(b));
            }
            else if (op == "arctan")
            {
                return Math.Atan(double.Parse(b));
            }
            else if (op == "~")
            {
                return double.Parse("-" + b);
            }
            else if (op == "^")
            {
                return Math.Pow(double.Parse(a), double.Parse(b));
            }
            else if (op == "!")
            {
                return Factorial(int.Parse(b));
            }
            else if (op == "sqrt")
            {
                return Math.Sqrt(double.Parse(b));
            }
            else
            {
                throw new System.ArgumentException("Error in evaluating expression!", "original");
            }
        }

        public static void OperatorFuncs(List<string> operatorFuncs)
        {
            /*
             * Adds operators that are functions (sin, cos, etc.) to
             * a list so that when we evaluate the postfix expression,
             * we will only pop the top value for these functions
             */

            operatorFuncs.Add("sqrt");
            operatorFuncs.Add("!");
            operatorFuncs.Add("~");
            operatorFuncs.Add("cos");
            operatorFuncs.Add("sin");
            operatorFuncs.Add("tan");
            operatorFuncs.Add("arccos");
            operatorFuncs.Add("arcsin");
            operatorFuncs.Add("arctan");
        }

        public static void Operators(Dictionary<string, int> operators)
        {
            /*
             * Adds operators to dictionary with each key defining the
             * relative order of operations in comparison with other
             * operators
             */

            operators.Add("sqrt", 3);
            operators.Add("!", 3);
            operators.Add("^", 3);
            operators.Add("~", 3);
            operators.Add("cos", 3);
            operators.Add("sin", 3);
            operators.Add("tan", 3);
            operators.Add("arccos", 3);
            operators.Add("arcsin", 3);
            operators.Add("arctan", 3);
            operators.Add("*", 2);
            operators.Add("/", 2);
            operators.Add("%", 2);
            operators.Add("+", 1);
            operators.Add("-", 1);
        }
        private static double Factorial(int n)
        {
            /*
             * Recursively calculates the factorial function
             * of an integer n.
             */

            if (n == 1)
            {
                return 1;
            }
            else
            {
                return n * Factorial(n - 1);
            }
        }
    }
}
