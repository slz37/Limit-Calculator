using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Limit_Calculator
{
    class OperatorFunctions
    {
        /// <summary>
        /// Evaluate a given operator on values.Converts each
        /// input from a string to their equivalent mathematical
        /// expression. Doesn't contain every function, but should
        /// capture most of them.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="op"></param>
        /// <returns></returns>
        public static double EvaluateExp(string a, string b, string op)
        {
            if (op == "+")
            {
                return double.Parse(a) + double.Parse(b);
            }
            else if (op == "-")
            {
                double.Parse(a);
                double.Parse(b);
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
            else if (op == "cot")
            {
                return 1 / Math.Tan(double.Parse(b));
            }
            else if (op == "sec")
            {
                return 1 / Math.Cos(double.Parse(b));
            }
            else if (op == "csc")
            {
                return 1 / Math.Sin(double.Parse(b));
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
                return MathFunctions.Gamma(double.Parse(b));
            }
            else if (op == "sqrt")
            {
                return Math.Sqrt(double.Parse(b));
            }
            else if (op == "ln")
            {
                return Math.Log(double.Parse(b));
            }
            else if (op == "abs")
            {
                return Math.Abs(double.Parse(b));
            }
            else if (op == "log")
            {
                return Math.Log(double.Parse(b), double.Parse(a));
            }
            else
            {
                throw new System.ArgumentException("Error in evaluating expression!", "original");
            }
        }

        /// <summary>
        /// Adds operators that are functions(sin, cos, etc.) to
        /// a list so that when we evaluate the postfix expression,
        /// we will only pop the top value for these functions.
        /// </summary>
        /// <param name="singleVarFuncs"></param>
        public static void UnaryOperators(List<string> unaryOperators)
        {
            //unaryOperators.Add("log");
            unaryOperators.Add("abs");
            unaryOperators.Add("ln");
            unaryOperators.Add("sqrt");
            unaryOperators.Add("!");
            //unaryOperators.Add("~");
            unaryOperators.Add("cos");
            unaryOperators.Add("sin");
            unaryOperators.Add("tan");
            unaryOperators.Add("arccos");
            unaryOperators.Add("arcsin");
            unaryOperators.Add("arctan");
            unaryOperators.Add("cot");
            unaryOperators.Add("sec");
            unaryOperators.Add("csc");
        }

        /// <summary>
        /// Adds operators that are functions(+, -, *, etc.) to
        /// a list so that when we evaluate the postfix expression,
        /// we will use two operands.
        /// </summary>
        /// <param name="singleVarFuncs"></param>
        public static void BinaryOperators(List<string> binaryOperators)
        {
            binaryOperators.Add("log");
            binaryOperators.Add("^");
            binaryOperators.Add("*");
            binaryOperators.Add("/");
            binaryOperators.Add("%");
            binaryOperators.Add("+");
            binaryOperators.Add("-");
        }

        /// <summary>
        /// Adds operators to dictionary with each key defining the
        /// relative order of operations in comparison with other
        /// operators.
        /// </summary>
        /// <param name="operators"></param>
        public static void Operators(Dictionary<string, int> operators)
        {
            operators.Add("log", 4);
            operators.Add("abs", 4);
            operators.Add("ln", 4);
            operators.Add("sqrt", 4);
            operators.Add("!", 4);
            //operators.Add("~", 1);
            operators.Add("cos", 4);
            operators.Add("sin", 4);
            operators.Add("tan", 4);
            operators.Add("arccos", 4);
            operators.Add("arcsin", 4);
            operators.Add("arctan", 4);
            operators.Add("cot", 4);
            operators.Add("sec", 4);
            operators.Add("csc", 4);
            operators.Add("^", 3);
            operators.Add("*", 2);
            operators.Add("/", 2);
            operators.Add("%", 2);
            operators.Add("+", 1);
            operators.Add("-", 1);
        }
    }
}
