using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Limit_Calculator
{
    class MathFunctions
    {
        public static double Factorial(int n)
        {
            /*
             * Recursively calculates the factorial function
             * of an integer n.
             */

            if (n == 0)
            {
                return 1;
            }
            else
            {
                return n * Factorial(n - 1);
            }
        }

        public static double Gamma(double n)
        {
            /*
             * If the input is actually an integer,
             * calculate the factorial function as normal.
             * Otherwise, I use the Lanczos approximation
             * to calculate the gamma function.
             */

            //Check whether our input is actually an int
            bool isPosInt = ((Math.Abs(n % 1) <= (double.Epsilon * 100)) & (n > 0));

            if (isPosInt)
            {
                return Factorial(Convert.ToInt32(n));
            }
            else
            {
                /*
                * This is overkill since n will always be real, but it's
                * worth including the full approximation just in case.
                */
                return LanczosApprox(n);
            }
        }

        private static double LanczosApprox(double n)
        {
            double y;
            int g = 8; //Use 7 terms of the formula

            //Reflection formula to extend to Re(n) < 0.5
            if (n < 0.5)
            {
                /*
                * We aren't actually trying to calculate gamma(n) but rather
                * n! for non-positive integers to test for convergence of the
                * limit. In this context, we want gamma(0) -> 1 and not inf.
                */
                y = LanczosApprox(1 - n); 
                //y = Math.PI / (Math.Sin(Math.PI * n) * LanczosApprox(1 - n));
            }
            else
            {
                n -= 1; //This formulation calculates Gamma(n + 1), so subtract 1 to compensate
                y =  Math.Sqrt(2 * Math.PI) * Math.Pow((n + g - 0.5), n + 0.5) * Math.Exp(-(n + g - 0.5)) * S(n, g);
            }

            return y;
        }

        private static double S(double n, int g)
        {
            /*
             * Found at wikipedia for Lanczos approximation
             *  g = 7 and n = 9
             */
            double[] p = {0.99999999999980993,
                676.5203681218851,
                -1259.1392167224028,
                771.32342877765313,
                -176.61502916214059,
                12.507343278686905,
                -0.13857109526572012,
                9.9843695780195716e-6,
                1.5056327351493116e-7 };
            double S = p[0];

            //Sum coefficients to get S
            int i = -1;
            foreach (double pval in p)
            {
                if (i >= 0)
                {
                    S += pval / (n + i + 1);
                }
                i += 1;
            }
            return S;
        }
    }
}
