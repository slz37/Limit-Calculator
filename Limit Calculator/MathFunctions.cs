using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Limit_Calculator
{
    class MathFunctions
    {
        /// <summary>
        /// Takes in a string of an expression and splits
        /// it up to feed into the derivative function
        /// to properly calculate derivatives.
        /// </summary>
        /// <param name="func"></param>
        /// <returns></returns>
        public static string Derivative(string func)
        {
            string funcDer = "";
            Queue<string> queue = new Queue<string>();

            //Base Operator
            string[] baseOperators = {"+", "-"};

            //Iterate over all chars in the string
            foreach (char item in func)
            {
                string token = item.ToString();
                if (token == "(")
                {
                    queue.Enqueue(token);
                }
                else if (token == ")")
                {
                    string tempStr = "";

                    //Get all items in stack up to first "(" or base operator
                    while (queue.Count > 0)
                    {
                        if ((queue.Peek() != "(") || (baseOperators.Contains(queue.Peek())))
                        {
                            tempStr += queue.Dequeue();
                        }
                    }

                    //Take derivative and then put it back onto stack with operator
                    queue.Enqueue(EvalDerivative(tempStr));
                    queue.Enqueue(token);
                }
                else if (baseOperators.Contains(token))
                {
                    string tempStr = "";

                    //Get all items in stack up to first "("
                    while (queue.Count > 0)
                    {
                        if (queue.Peek() != "(")
                        {
                            tempStr += queue.Dequeue();
                        }
                        else
                        {
                            queue.Dequeue();
                        }
                    }

                    //Take derivative and then put it back onto stack with operator
                    queue.Enqueue(EvalDerivative(tempStr));
                    queue.Enqueue(token);
                }
                else
                {
                    queue.Enqueue(token);
                }
            }

            //Now clear stack and put it all together
            while (queue.Count > 0)
            {
                funcDer += queue.Dequeue();
            }

            return funcDer;
        }

        /// <summary>
        /// Calculates the symbolic derivative of the input function.
        /// </summary>
        /// <param name="func"></param>
        /// <returns></returns>
        public static string EvalDerivative(string func)
        {
            /*
             * Gonna need a stack of some sort
             * Probably split recursively on each
             * operator level then calculate derivative
             * and bring it all together
             */
            string tempStr = "";
            Stack<string> stack = new Stack<string>();
            Queue<string> queue = new Queue<string>();

            Dictionary<string, int> operators = new Dictionary<string, int>();
            OperatorFunctions.Operators(operators);

            //Iterate over all char in string
            for (int i = 0; i < func.Length; i++)
            {
                string token = func[i].ToString();

                //Once operator reached, perform derivative
                if (stack.Count > 0)
                {
                    if (!(operators.ContainsKey(token)))
                    {
                        stack.Push(token);
                        queue.Enqueue(token);
                        continue;
                    }
                    else
                    {
                        //Take the derivative of the operator
                        if (token == "^")
                        {
                            queue.Enqueue(token);
                            stack.Push(token);

                            //Decrease digit by 1
                            string nextToken = func[i + 1].ToString();
                            string digit = (double.Parse(nextToken) - 1).ToString();

                            tempStr += nextToken;

                            //Clear queue/stack
                            while (stack.Count > 0)
                            {
                                stack.Pop();
                                tempStr += queue.Dequeue();
                            }

                            //Skip next token
                            tempStr += digit;
                            i++;
                        }
                    }
                }
                else
                {
                    queue.Enqueue(token);
                    stack.Push(token);
                }
            }

            return tempStr;
        }

        /// <summary>
        /// Recursively calculates the factorial function of an integer n.
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        public static double Factorial(int n)
        {
            if (n == 0)
            {
                return 1;
            }
            else
            {
                return n * Factorial(n - 1);
            }
        }

        /// <summary>
        /// Calculates the double factorial of an integer n.
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        public static double DoubleFac(double n)
        {
            if (n <= 1)
            {
                return 1;
            }
            else
            {
                return n * DoubleFac(n - 2);
            }
        }

        /// <summary>
        /// Calculates the combination of two integers n and k.
        /// </summary>
        /// <param name="n"></param>
        /// <param name="k"></param>
        /// <returns></returns>
        public static double Combination(int n, int k)
        {
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

        /// <summary>
        /// Calculates gamma(n), where gamma is an extension of
        /// the factorial function to the complex domain.
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Estimates gamma(n) for any complex number using the
        /// Lanczos Approximation method.
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Calculates the S term for the Lanczos approximation.
        /// </summary>
        /// <param name="n"></param>
        /// <param name="g"></param>
        /// <returns></returns>
        private static double S(double n, int g)
        {
            //Lanczos coefficients - calculated from the line below
            //double[] p = LanczosCoefficients.Calculate(8, g - 1);
            List<double> p = new List<double>{0.999999999985635,
            676.52036812188,
            -1259.13921672634,
            771.323428720698,
            -176.615029376393,
            12.5073443051339,
            -0.138574190812469,
            1.06954947029389E-05};

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

        /// <summary>
        /// For a NxM matrix A and MxP matrix B, performs the matrix 
        /// multiplication to calculate a resultant matrix of size NxP.
        /// </summary>
        /// <param name="A"></param>
        /// <param name="B"></param>
        /// <returns></returns>
        public static double[,] MatrixMultiplication(double[,] A, double[,] B)
        {
            int N = A.GetLength(0);
            int M = B.GetLength(0);
            int P = B.GetLength(1);
            double[,] resultant = new double[N, P];

            //Not NxM MxP
            if (A.GetLength(1) != M)
            {
                throw new System.ArgumentException("Matrices do not share inner dimension!", "original");
            }

            //Calculate resulting values and add to new matrix
            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < P; j++)
                {
                    for (int k = 0; k < M; k++)
                    {
                        resultant[i, j] += A[i, k] * B[k, j];
                    }
                }
            }

            return resultant;
        }

        /// <summary>
        /// Takes in a NxM matrix that should be a column vector
        /// and removes the extraneous dimension.
        /// </summary>
        /// <param name="Matrix"></param>
        /// <returns></returns>
        public static double[] ReduceDimensions(double[,] Matrix)
        {
            int N = Matrix.GetLength(0);
            int M = Matrix.GetLength(1);
            double[] newMatrix = new double[N];

            //Iterate over every element and remove extra dimensions
            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < M; j++)
                {
                    if (Matrix[i, j] == 0)
                    {
                        continue;
                    }
                    else
                    {
                        newMatrix[i] = Matrix[i, j];
                    }
                }
            }

            return newMatrix;
        }
    }
}
