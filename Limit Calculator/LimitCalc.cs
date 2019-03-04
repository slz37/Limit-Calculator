using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using MathNet.Numerics;

namespace Limit_Calculator
{
    class LimitCalc
    {
        /// <summary>
        /// Calculates the limit as it approaches the value from both 
        /// the left and right and averages between the two answers 
        /// until either the x values get too close or the threshold
        /// is reached.
        /// </summary>
        /// <param name="func"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        static double EvaluateLimit(string func, double limit)
        {
            //Constants
            const double thresh = 1e-3;
            const double thresh_x = 1e-5;
            const double delta = 1e-4;

            //Initial step to initialize everything
            double x_l = limit - 1;
            double x_r = limit + 1;
            double left_ans = Calculator.Calculate(func, x_l);
            double right_ans = Calculator.Calculate(func, x_r);
            double del = Math.Abs(right_ans - left_ans);
            double del_x = Math.Abs(x_l - x_r);
            double ans = (left_ans + right_ans) / 2;

            //Infinite limits
            if (double.IsInfinity(limit))
            {
                //Deal with undefined fractions e.g. 1/inf with L'Hopital
                if (double.IsNaN(ans))
                {
                    //Get index of first outer division symbol
                    int index = StringFunctions.FindOuterSlash(func);

                    //Split into numerator and denominator
                    string num = func.Substring(0, index);
                    string den = func.Substring(index + 1, func.Length - 2);

                    //Take the derivate of the numerator and denominator
                    string numDer = MathFunctions.Derivative(num);
                    string denDer = MathFunctions.Derivative(den);

                    string tot = numDer + "/" + denDer;

                    return EvaluateLimit(tot, limit);
                }
                else
                {
                    return ans;
                }
            }

            //Loop until confident limit either converges or DNC
            while (true)
            {
                //If x values too close and still not converged, limit DNC
                if ((del < thresh) & (del_x < thresh))
                {
                    return ans;
                }
                if (del_x < thresh_x )
                {
                    return double.PositiveInfinity;
                }

                //Step to next values
                x_l += delta;
                x_r -= delta;

                //Calculate function at current step
                left_ans = Calculator.Calculate(func, x_l);
                right_ans = Calculator.Calculate(func, x_r);

                //Get differences for break conditions
                del = Math.Abs(right_ans - left_ans);
                del_x = Math.Abs(x_l - x_r);

                //Take average of two as current solution
                ans = (left_ans + right_ans) / 2;

                //Console.WriteLine(x_l + " " + x_r + " " + del_x + " " + left_ans + " " + right_ans + " " + del + " " + ans);
            }
        }

        /// <summary>
        /// Takes in user input for a function and a value to evaluate the limit at
        /// and returns the value of the function if it converges or returns
        /// "inf" if it diverges.
        /// </summary>
        /// <param name="args"></param>
        static void Main2(string[] args)
        {
            string func, limStr, limPostFix;
            double lim = 0, ans;

            //Operator dictionary to define order of operations
            Dictionary<string, int> operators = new Dictionary<string, int>();
            OperatorFunctions.Operators(operators);

            //Let user keep entering functions and limits as they desire
            while (true)
            {
                //User input
                Console.WriteLine("Input f(x): ");
                func = Console.ReadLine();
                Console.WriteLine("Input limit: ");
                limStr = Console.ReadLine();

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
                ans = EvaluateLimit(func, lim);
                Console.WriteLine("Limit as x->" + lim + " of " + func + " = " + Math.Round(ans, 3));
                Console.ReadLine();
            }
        }

        /// <summary>
        /// Test function for calculating the derivative
        /// of a postfix expression.
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            string func = "(x^7+2*x)";
            string funcPostFix = Calculator.Convert2Postfix(func);
            string test = MathFunctions.Derivative(funcPostFix);

            Console.WriteLine(test);
            Console.ReadLine();
        }
    }
}
