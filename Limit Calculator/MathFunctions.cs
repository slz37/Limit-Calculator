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
                //n -= 1; //This formulation calculates Gamma(n + 1), so subtract 1 to compensate
                y =  Math.Sqrt(2 * Math.PI) * Math.Pow((n + g - 0.5), n + 0.5) * Math.Exp(-(n + g - 0.5)) * S(n, g);
            }

            return y;
        }

        private static double S(double n, int g)
        {
            /*
             * Calculates the S term for the Lanczos
             * approximation
             */

            //Lanczos coefficients
            List<List<double>> p = LanczosCoefficients.Calculate(8, g - 1);
            
            double S = p[0][0];

            //Sum coefficients to get S
            int i = -1;
            foreach (List<double> pval in p)
            {
                if (i >= 0)
                {
                    S += pval[0] / (n + i + 1);
                }
                i += 1;
            }
            return S;
        }

        public static double DoubleFac(double n)
        {
            /*
             * Calculates the double factorial of an
             * integer n
             */

            if (n <= 1)
            {
                return 1;
            }
            else
            {
                return n * DoubleFac(n - 2);
            }
        }
        public static List<List<double>> MatrixMultiplication(List<List<double>> A, List<List<double>> B)
        {
            /*
             * For a NxM matrix A and MxP matrix B, performs the matrix multiplication
             * to calculate a resultant matrix of size NxP.
             */
            List<List<double>> resultant = new List<List<double>>();
            double N = A.Count();
            double M = B.Count();
            double P = B[0].Count();

            //Not NxM MxP
            if (A[0].Count() != B.Count())
            {
                throw new System.ArgumentException("Matrices do not share inner dimension!", "original");
            }

            //Initialize resultant matrix
            for (int i = 0; i < N; i++)
            {
                //Add new row
                resultant.Add(new List<double>());

                for (int j = 0; j < P; j++)
                {
                    resultant[i].Insert(j, 0);
                }
            }


            //Calculate resulting values and add to new matrix
            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < P; j++)
                {
                    for (int k = 0; k < M; k++)
                    {
                        resultant[i][j] += A[i][k] * B[k][j];
                    }
                }
            }

            return resultant;
        }

        public static double Combination(int n, int k)
        {
            /*
             * Calculates the combination of two integers
             * n and k
             */

            if ((n == 0) & (k == 0))
            {
                return 1;
            }
            else if ((k > n) || (n == 0) || (k < 0))
            {
                return 0;
            }
            else
            {
                return MathFunctions.Factorial(n) / (MathFunctions.Factorial(k) * (MathFunctions.Factorial(n - k)));
            }
        }
    }
}
