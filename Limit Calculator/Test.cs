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
            //Debug flags
            bool debugLimit = false;
            bool debugDerivative = true;

            if (debugLimit)
            {
                string func, limStr, limPostFix;
                double lim = 0, ans;

                //Operator dictionary to define order of operations
                Dictionary<string, int> operators = new Dictionary<string, int>();
                OperatorFunctions.Operators(operators);

                //Test as many different operators I can think of - probably don't need test suite for this
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
                Console.WriteLine("Answer from Wolfram: " + 3.166983673);
                Console.ReadLine();
            }

            if (debugDerivative)
            {
                string[] funcList = {"x+x+x-x",
                                     "ln(1/x)",
                                     "(x+2)/(x-5)",
                                     "(x*2)+(x/5)",
                                     "2+(x+2)^2+2^x",
                                    };

                //Run through test suite
                foreach (string func in funcList)
                {
                    try
                    {
                        string funcPostFix = Calculator.Convert2Postfix(func);
                        string test = DerivativeCalculator.Derivative(funcPostFix);

                        //Derivatives
                        Console.WriteLine(func + ": " + Calculator.Calculate(test, 2) + "\n");
                    }
                    catch
                    {
                        Console.WriteLine("Error with derivative of: " + func + "\n");
                    }
                }
                Console.ReadLine();
            }
        }
    }
}
