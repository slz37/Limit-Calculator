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
            bool debugIsComplete = false;

            if (debugLimit)
            {
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
                Console.WriteLine("Answer from Wolfram: " + 3.166983673);
                Console.ReadLine();
            }

            if (debugDerivative)
            {
                string func = "2+x";
                string funcPostFix = Calculator.Convert2Postfix(func);
                string test = DerivativeCalculator.Derivative(funcPostFix);

                //Derivative of 2+x evaluated at 2, both outputs should be equal
                Console.WriteLine(Calculator.Calculate("1", 2));
                Console.WriteLine(Calculator.Calculate(test, 2));
                Console.ReadLine();

                /*
                string func2 = "2+(x+2)^2+2^x";
                string funcPostFix2 = Calculator.Convert2Postfix(func2);
                string test2 = DerivativeCalculator.Derivative(funcPostFix2);

                //Derivative of 2+(x+2)^2+2^x evaluated at 2, both outputs should be equal
                Console.WriteLine(Calculator.Calculate("2*(x+2)+(2^x*ln(2))", 2));
                Console.WriteLine(Calculator.Calculate(test2, 2));
                Console.ReadLine();
                */
            }
            
            if (debugIsComplete)
            {
                string func = "^ x 2";
                string funcRev = StringFunctions.ReverseString(func);
                string funcEval = StringFunctions.ReplaceConstants(funcRev, Math.Tan(2 + Math.Exp(10)) + Math.PI);

                //Console should show the value it evaluates to
                Console.WriteLine(Calculator.EvaluatePostFix(funcEval));
                Console.WriteLine(DerivativeCalculator.IsComplete(func));
                Console.Read();
            }
        }
    }
}
