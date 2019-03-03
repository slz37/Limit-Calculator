using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Limit_Calculator
{
    class LanczosCoefficients
    {
        private static double S(int i, int j)
        {
            /*
             * Returns the summation of combinations for two integers
             * i and j to calculate the C matrices
             */

            double sum = 0;
            for (int k = 0; k <= i; k++)
            {
                sum += MathFunctions.Combination(2 * i, 2 * k) * MathFunctions.Combination(k, k + j - i);
            }

            return sum;
        }

        private static List<List<double>> MakeB(double n)
        {
            /*
             * Calculates the B matrix to determine the coefficients
             * for the Lanczos Approximation
             */

            List<List<double>> B = new List<List<double>>();

            //Iterate over all rows and columns to create matrix
            for (int i = 0; i < n; i++)
            {
                //Add new row
                B.Add(new List<double>());

                //Add all column values to that row
                for (int j = 0; j < n; j++)
                {
                    if (i == 0)
                    {
                        B[0].Insert(j, 1);
                    }
                    else if ((i > 0) & (j >= i))
                    {
                        B[i].Insert(j, Math.Pow(-1, j - i) * MathFunctions.Combination(i + j - 1, j - i));
                    }
                    else
                    {
                        B[i].Insert(j, 0);
                    }
                }
            }

            return B;
        }

        private static List<List<double>> MakeC(double n)
        {
            /*
             * Calculates the C matrix to determine the coefficients
             * for the Lanczos Approximation
             */

            List<List<double>> C = new List<List<double>>();

            //Iterate over all rows and columns to create matrix
            for (int i = 0; i < n; i++)
            {
                //Add new row
                C.Add(new List<double>());

                //Add all column values to that row
                for (int j = 0; j < n; j++)
                {
                    if ((i == 0) & (j == 0))
                    {
                        C[i].Insert(j, 0.5);
                    }
                    else if (j > i)
                    {
                        C[i].Insert(j, 0);
                    }
                    else
                    {
                        C[i].Insert(j, Math.Pow(-1, i - j) * S(i, j));
                    }
                }
            }

            return C;
        }

        private static List<List<double>> MakeDr(double n)
        {
            /*
             * Calculates the Dr matrix to determine the coefficients
             * for the Lanczos Approximation
             */

            List<List<double>> D = new List<List<double>>();

            //Initialize D matrix
            for (int i = 0; i < n; i++)
            {
                //Add new row
                D.Add(new List<double>());

                for (int j = 0; j < n; j++)
                {
                    D[i].Insert(j, 0);
                }
            }

            //Base values
            D[0][0] = 1;
            D[1][1] = -1;

            //Add values to matrix
            for (int i = 2; i < n; i++)
            {
                D[i][i] = (D[i - 1][i - 1] * (2 * (2 * i - 1))) / (i - 1);
            }

            return D;
        }

        private static List<List<double>> MakeDc(double n)
        {
            /*
             * Calculates the Dc matrix to determine the coefficients
             * for the Lanczos Approximation
             */

            List<List<double>> D = new List<List<double>>();

            //Initialize D matrix
            for (int i = 0; i < n; i++)
            {
                //Add new row
                D.Add(new List<double>());

                for (int j = 0; j < n; j++)
                {
                    D[i].Insert(j, 0);
                }
            }

            //Add values to matrix
            for (int i = 0; i < n; i++)
            {
                D[i][i] = 2 * MathFunctions.DoubleFac(2 * i - 1);
            }

            return D;
        }

        private static List<List<double>> MakeF(double n, double g)
        {
            /*
             * Calculates the F vector to determine the coefficients
             * for the Lanczos Approximation
             */

            List<List<double>> F = new List<List<double>>();

            for (int i = 0; i < n; i++)
            {
                F.Add(new List<double>());

                F[i].Insert(0, Math.Sqrt(2) * Math.Pow(Math.E / (2 * (i + g) + 1), i + 0.5));
            }

            return F;
        }

        private static List<List<double>> MakeP(List<List<double>> B, List<List<double>> C, List<List<double>> Dr, List<List<double>> Dc, List<List<Double>> F)
        {
            /*
             * Calculates the p coefficients for the
             * Lanczos Approximation
             */

            List<List<double>> p = new List<List<double>>();

            //Matrix multiplication
            p = MathFunctions.MatrixMultiplication(MathFunctions.MatrixMultiplication(MathFunctions.MatrixMultiplication(MathFunctions.MatrixMultiplication(Dr, B), C), Dc), F);

            return p;
        }

        public static List<List<double>> Calculate(double n, double g)
        {
            /*
             * Calculates the coefficients for the Lanczos
             * approximation to the gamma function
             */

            List<List<double>> B = new List<List<double>>();
            List<List<double>> C = new List<List<double>>();
            List<List<double>> Dr = new List<List<double>>();
            List<List<double>> Dc = new List<List<double>>();
            List<List<double>> F = new List<List<double>>();
            List<List<double>> P = new List<List<double>>();

            //Create nxn matrices and nx1 F vector
            B = MakeB(n);
            C = MakeC(n);
            Dr = MakeDr(n);
            Dc = MakeDc(n);
            F = MakeF(n, g);
            P = MakeP(B, C, Dr, Dc, F);

            //Multiple by scaling factors
            int i = 0;
            foreach (List<double> element in P)
            {
                P[i][0] = element[0] * Math.Exp(g) / Math.Sqrt(2 * Math.PI);

                i++;
            }

            return P;
        }
    }
}
