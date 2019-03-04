using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Limit_Calculator
{
    class Test
    {
        /// <summary>
        /// Test function for ensuring that certain methods
        /// function properly
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            bool debugLimit = false;
            bool debugDerivative = true;

            if (debugLimit)
            {
                //Code to test limit method
                string func, limStr, limPostFix;
                double lim = 0, ans;


                //Operator dictionary to define order of operations
                Dictionary<string, int> operators = new Dictionary<string, int>();
                OperatorFunctions.Operators(operators);

                //Test as many different operators I can think of
                func = "-5 + (((sqrt(x) - x!)^2 / (x^2 * 5 + 3 * csc(cos(x)))) % 2) + abs(x) + ln(x) + arctan(x)";
                limStr = "5";

                //Create list of elements in limit to test for operators
                List<string> limList = new List<string>();
                limList = StringFunctions.Convert2List(limStr);

                //Check for operators in limit and convert appropriately
                if (limList.Any(operators.ContainsKey))
                {
                    limStr = StringFunctions.ReplaceConstants(limStr);
                    limPostFix = Calculator.Convert2Postfix(limStr);
                    lim = Calculator.EvaluatePostFix(limPostFix);
                }
                else
                {
                    limStr = StringFunctions.ReplaceConstants(limStr);
                    lim = double.Parse(limStr);
                }

                //Remove any whitespace for parsing
                func = func.Replace(" ", "");

                //Calculate limits and output answer
                ans = LimitCalc.EvaluateLimit(func, lim);
                Console.WriteLine("Limit as x->" + lim + " of " + func + " = " + Math.Round(ans, 3));
                Console.ReadLine();
            }

            if (debugDerivative)
            {
                //Code to test derivative method
                string func2 = "x^3-1";
                string funcPostFix = Calculator.Convert2Postfix(func2);
                string test = MathFunctions.Derivative(funcPostFix);

                //Derivative of x^3-1 evaluated at 2, both outputs should be equal
                Console.WriteLine(Calculator.Calculate("12", 2));
                Console.WriteLine(Calculator.Calculate(test, 2));
                Console.ReadLine();
            }
        }
    }
}
