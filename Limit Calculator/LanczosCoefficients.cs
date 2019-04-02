using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Limit_Calculator
{
    class LanczosCoefficients
    {
        /// <summary>
        /// Returns the summation of combinations for two integers
        /// i and j to calculate the C matrices.
        /// </summary>
        /// <param name="i"></param>
        /// <param name="j"></param>
        /// <returns></returns>
        private static double S(int i, int j)
        {
            double sum = 0;
            for (int k = 0; k <= i; k++)
            {
                sum += MathFunctions.Combination(2 * i, 2 * k) * MathFunctions.Combination(k, k + j - i);
            }

            return sum;
        }

        /// <summary>
        /// Calculates the B matrix to determine the coefficients
        /// for the Lanczos Approximation.
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        private static double[,] MakeB(int n)
        {
            double[,] B = new double[n, n];

            //Iterate over all rows and columns to create matrix
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    if (i == 0)
                    {
                        B[0, j] = 1;
                    }
                    else if ((i > 0) & (j >= i))
                    {
                        B[i, j] = Math.Pow(-1, j - i) * MathFunctions.Combination(i + j - 1, j - i);
                    }
                    else
                    {
                        B[i, j] = 0;
                    }
                }
            }

            return B;
        }

        /// <summary>
        /// Calculates the C matrix to determine the coefficients
        /// for the Lanczos Approximation.
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        private static double[,] MakeC(int n)
        {
            double[,] C = new double[n, n];

            //Iterate over all rows and columns to create matrix
            for (int i = 0; i < n; i++)
            {
                //Add all column values to that row
                for (int j = 0; j < n; j++)
                {
                    if ((i == 0) & (j == 0))
                    {
                        C[0, 0] = 0.5;
                    }
                    else if (j > i)
                    {
                        C[i, j] = 0;
                    }
                    else
                    {
                        C[i, j] =  Math.Pow(-1, i - j) * S(i, j);
                    }
                }
            }

            return C;
        }

        /// <summary>
        /// Calculates the Dr matrix to determine the coefficients
        /// for the Lanczos Approximation.
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        private static double[,] MakeDr(int n)
        {
            double[,] D = new double[n, n];

            //Base values
            D[0, 0] = 1;
            D[1, 1] = -1;

            //Add values to matrix
            for (int i = 2; i < n; i++)
            {
                D[i, i] = (D[i - 1, i - 1] * (2 * (2 * i - 1))) / (i - 1);
            }

            return D;
        }

        /// <summary>
        /// Calculates the Dc matrix to determine the coefficients 
        /// for the Lanczos Approximation.
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        private static double[,] MakeDc(int n)
        {
            double[,] D = new double[n, n];

            //Add values to matrix
            for (int i = 0; i < n; i++)
            {
                D[i, i] = 2 * MathFunctions.DoubleFac(2 * i - 1);
            }

            return D;
        }

        /// <summary>
        /// Calculates the F vector to determine the coefficients
        /// for the Lanczos Approximation.
        /// </summary>
        /// <param name="n"></param>
        /// <param name="g"></param>
        /// <returns></returns>
        private static double[,] MakeF(int n, double g)
        {
            double[,] F = new double[n, n];

            for (int i = 0; i < n; i++)
            {
                F[i, 0] = Math.Sqrt(2) * Math.Pow(Math.E / (2 * (i + g) + 1), i + 0.5);
            }

            return F;
        }

        /// <summary>
        /// Calculates the p coefficients for the
        /// Lanczos Approximation.
        /// </summary>
        /// <param name="B"></param>
        /// <param name="C"></param>
        /// <param name="Dr"></param>
        /// <param name="Dc"></param>
        /// <param name="F"></param>
        /// <returns></returns>
        private static double[] MakeP(double[,] B, double[,] C,
                                                               double[,] Dr, double[,] Dc,
                                                               double[,] F)
        {
            double[,] P = new double[B.Length, B.Length];
            double[] p = new double[B.Length];

            //Matrix multiplication
            P = MathFunctions.MatrixMultiplication(
                  MathFunctions.MatrixMultiplication(
                  MathFunctions.MatrixMultiplication(
                  MathFunctions.MatrixMultiplication(Dr, B), 
                                                                                            C),
                                                                                          Dc),
                                                                                             F);

            //Convert to column vector
            p = MathFunctions.ReduceDimensions(P);

            return p;
        }

        /// <summary>
        /// Calculates the coefficients for the Lanczos
        /// approximation to the gamma function.
        /// </summary>
        /// <param name="n"></param>
        /// <param name="g"></param>
        /// <returns></returns>
        public static double[] Calculate(int n, double g)
        {
            double[,] B = new double[n, n];
            double[,] C = new double[n, n];
            double[,] Dr = new double[n, n];
            double[,] Dc = new double[n, n];
            double[,] F = new double[n, n];
            double[] P = new double[n];
            double[] p = new double[n];

            //Create nxn matrices and nx1 vectors
            B = MakeB(n);
            C = MakeC(n);
            Dr = MakeDr(n);
            Dc = MakeDc(n);
            F = MakeF(n, g);
            P = MakeP(B, C, Dr, Dc, F);

            //Multiple by scaling factors
            int i = 0;
            foreach (double element in P)
            {
                p[i] = element * Math.Exp(g) / Math.Sqrt(2 * Math.PI);

                i++;
            }

            return p;
        }
    }
}
