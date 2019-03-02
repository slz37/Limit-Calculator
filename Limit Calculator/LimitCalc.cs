﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace Limit_Calculator
{
    class LimitCalc
    {
        static double EvaluateLimit(string func, double limit)
        {
            /*
            * Calculates the limit as it approaches the value from both 
            * the left and right and averages between the two answers 
            * until either the x values get too close or the threshold
            * is reached
            */

            //Constants
            const double thresh = 1e-3;
            const double thresh_x = 1e-5;
            const double delta = 0.0001;

            //Initial step to initialize everything
            double x_l = limit - 1;
            double x_r = limit + 1;
            double left_ans = Calculator.Calculate(func, x_l);
            double right_ans = Calculator.Calculate(func, x_r);
            double del = Math.Abs(right_ans - left_ans);
            double del_x = Math.Abs(x_l - x_r);
            double ans = (left_ans + right_ans) / 2;

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
                    return double.NaN;
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
            }
        }
        static void Main(string[] args)
        {
            /*
            * Takes in user input for a function and a value to evaluate the limit at
            * and returns the value of the function if it converges or returns "Does
            * not converge" if it diverges
            */
            string func, limStr;
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

                List<string> limList = new List<string>();
                limList = StringFunctions.Convert2List(limStr);

                //Check for operators in limit and convert appropriately
                if (limList.Any(operators.ContainsKey))
                {
                    limStr = StringFunctions.ReplaceConstants(limStr);
                    lim = Calculator.Convert2Postfix(limStr);
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
                Console.WriteLine("Limit as x->" + lim + " of " + func + " = " + Math.Round(ans, 5));
                Console.ReadLine();
            }
        }
    }
}
