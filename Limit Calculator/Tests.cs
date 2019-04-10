using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Limit_Calculator
{
    class Tests
    {
        /// <summary>
        /// Tests for finding the limit of many
        /// functions and comparing with results
        /// found from wolframalpha.com
        /// </summary>
        public static void debugLimit()
        {
            Console.WriteLine("Debugging limit methods:");
            string limStr, limPostFix;
            double lim = 0;

            //Operator dictionary to define order of operations
            Dictionary<string, int> operators = new Dictionary<string, int>();
            OperatorFunctions.Operators(operators);

            //Test as many different operators I can think of - probably don't need test suite for this
            string[] funcList = { "-5 + (((sqrt(x) - x!)^2 / (x^2 * 5 + 3 * csc(cos(x)))) % 2) + abs(x) + ln(x) + arctan(x)",
                                        "-(x+5)",
                                        "x + -5",
                                        "(-x)",
                                        "-x + 5",
                                    };
            double[] correctAns = {3.166983673,
                                        -10,
                                        0,
                                        -5,
                                        0,
                                        };

            limStr = "5";
            for (int i = 0; i < funcList.Length; i++)
            {
                string func = funcList[i];
                double ans = correctAns[i];

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
                double test = LimitCalc.EvaluateLimit(func, lim);
                Console.WriteLine("Limit as x->" + lim + " of " + func + " = " + Math.Round(test, 3) + " ans: " + ans);
            }
        }

        /// <summary>
        /// Debugs the IsComplete function, which
        /// checks whether a given string of operators
        /// and operands can be computed.
        /// </summary>
        public static void debugIsComplete()
        {
            Console.WriteLine("Debugging IsComplete method:");
            string func = "ln 2";
            //Calculator.EvaluatePostFix
            bool testComplete = DerivativeCalculator.IsComplete(func);
            Console.WriteLine(testComplete);
        }

        /// <summary>
        /// Debugs the derivative function, checking
        /// against solutions found from wolframalpha.com
        /// in order to compute L'Hopital's rule for
        /// taking limits that result in an indeterminate
        /// form.
        /// </summary>
        public static void debugDerivative()
        {
            Console.WriteLine("Debugging derivative methods:");
            string[] funcList = {"(x+1)^(x+1)",
                                     "(x+2)^2",
                                     "x^5",
                                     "2^(x+2)",
                                     "x+x+x-x",
                                     "ln(1/x)",
                                     "(x+2)/(x-5)",
                                     "(x*2)+(x/5)",
                                     "2+(x+2)^2+2^x",
                                     "e^x",
                                     "cos(2)",
                                     "cos(x)",
                                     "cos(x^2)",
                                     "x",
                                     "-x",
                                     "-2^x",
                                     "sin(5)",
                                     "sin(x)",
                                     "sin(x^2)",
                                     "tan(5)",
                                     "tan(x)",
                                     "tan(x^2)",
                                     };

            //From wolframalpha
            string[] correctAns = {"56.6625",
                                       "8",
                                       "80",
                                       "11.09035",
                                       "2",
                                       "-0.5",
                                       "-7/9",
                                       "2.2",
                                       "10.7726",
                                       "7.38905",
                                       "0",
                                       "-0.90929",
                                       "3.0272",
                                       "1",
                                       "-1",
                                       "-2.77258",
                                       "0",
                                       "-0.416146",
                                       "-2.614574",
                                       "0",
                                       "5.774399",
                                       "9.36220048"
                                       };

            //Run through test suite
            for (int i = 0; i < funcList.Length; i++)
            {
                try
                {
                    string func = funcList[i];
                    string ans = correctAns[i];

                    //Replace constants, convert to postfix, take derivative
                    string analytic_func = StringFunctions.ReplaceConstants(func, 0, false);
                    string funcPostFix = Calculator.Convert2Postfix(analytic_func);
                    string test = DerivativeCalculator.Derivative(funcPostFix);

                    Console.WriteLine(func + ": " + Calculator.Calculate(test, 2) + " ans: " + ans);
                }
                catch
                {
                    string func = funcList[i];

                    Console.WriteLine("Error with derivative of: " + func);
                }
            }
        }
    }
}
