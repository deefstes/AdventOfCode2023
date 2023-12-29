namespace AdventOfCode2023.Utils
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Text;

    public static class Utils
    {
        public static (T, TimeSpan) MeasureExecutionTime<T>(Func<T> function)
        {
            Stopwatch stopwatch = Stopwatch.StartNew();
            T result = function();
            stopwatch.Stop();

            return (result, stopwatch.Elapsed);
        }

        public static int Levenshtein(string s, string t)
        {
            int n = s.Length;
            int m = t.Length;
            int[,] d = new int[n + 1, m + 1];

            // Verify arguments.
            if (n == 0)
            {
                return m;
            }

            if (m == 0)
            {
                return n;
            }

            // Initialize arrays.
            for (int i = 0; i <= n; d[i, 0] = i++)
            {
            }

            for (int j = 0; j <= m; d[0, j] = j++)
            {
            }

            // Begin looping.
            for (int i = 1; i <= n; i++)
            {
                for (int j = 1; j <= m; j++)
                {
                    // Compute cost.
                    int cost = (t[j - 1] == s[i - 1]) ? 0 : 1;
                    d[i, j] = Math.Min(
                    Math.Min(d[i - 1, j] + 1, d[i, j - 1] + 1),
                    d[i - 1, j - 1] + cost);
                }
            }
            // Return cost.
            return d[n, m];
        }

        public static T Gcd<T>(T a, T b)
        {
            if (a == null)
                throw new ArgumentNullException(nameof(a));
            if (b == null)
                throw new ArgumentNullException(nameof(b));

            dynamic x = a;
            dynamic y = b;

            if (y == 0)
                return Math.Abs(x);
            else
                return Gcd(y, x % y);
        }

        public static T Lcm<T>(T[] values)
        {
            return values.Aggregate((a, b) => {
                if (a == null)
                    throw new ArgumentNullException(nameof(a));
                if (b == null)
                    throw new ArgumentNullException(nameof(b));

                dynamic x = a;
                dynamic y = b;
                return Math.Abs(x * y / Gcd(x, y));
            });
        }

        public static string GridToString(char[,] grid)
        {
            StringBuilder sb = new();
            for (int j = 0; j < grid.GetLength(1); j++)
            {
                for (int i = 0; i < grid.GetLength(0); i++)
                {
                    sb.Append(grid[i, j]);
                }
                sb.AppendLine();
            }

            return sb.ToString();
        }

        public static List<(T,T)> Combinations<T>(List<T> input)
        {
            List<(T, T)> output = [];

            for (int i = 0; i< input.Count;i++)
            {
                for (int j = i+1;j<input.Count;j++)
                    output.Add((input[j], input[i]));
            }

            return output;
        }

        /// <summary>
        /// Returns the values for all the coefficients (values for a) in a polynomial of the form
        /// y = a[n].x^n + a[n-1].x^(n-1) + ... + a[2].x^2 + a[1].x + a[0]
        /// given a list of n solutions for y and x where y=f(x)
        /// </summary>
        /// <param name="solutions">List of x and y solutions for y=f(x)</param>
        /// <returns>Array of coefficients from highest order (a[n]) to lowest order (a[0])</returns>
        public static double[] DiscoverPolynomial(List<(long y, long x)> solutions)
        {
            var order = solutions.Count;
            var coeffMatrix = new double[order, order + 1];

            for (int row = 0; row < order; row++)
            {
                coeffMatrix[row, order] = solutions[row].y;

                for (int n = 0; n < order; n++)
                {
                    var exp = Math.Pow(solutions[row].x, n);
                    coeffMatrix[row, order - n - 1] = exp;
                }
            }

            return SolveLinearEqSystem(coeffMatrix);
        }

        public static double SolvePolynomial(double[] coefficients, long x)
        {
            double y = 0;
            for (int i=0; i<coefficients.Length; i++)
                y += coefficients[i]*Math.Pow(x, coefficients.Length-i-1);

            return y;
        }

        /// <summary>Computes the solution of a linear equation system.</summary>
        /// <param name="matrix">
        /// The system of linear equations as an augmented matrix[row, col] where (rows + 1 == cols).
        /// It will contain the solution in "row canonical form" if the function returns "true".
        /// </param>
        /// <returns>Returns the list of constants.</returns>
        public static double[] SolveLinearEqSystem(double[,] matrix)
        {
            // input checks
            int rowCount = matrix.GetUpperBound(0) + 1;
            if (matrix == null || matrix.Length != rowCount * (rowCount + 1))
                throw new ArgumentException("The algorithm must be provided with a (n x n+1) matrix.");
            if (rowCount < 1)
                throw new ArgumentException("The matrix must at least have one row.");

            var workingMatrix = new double[rowCount, rowCount + 1];
            Array.Copy(matrix, workingMatrix, matrix.Length);
            var indeterminates = new double[rowCount];

            // pivoting
            for (int col = 0; col + 1 < rowCount; col++)
                if (workingMatrix[col, col] == 0) // check for zero coefficients
                {
                    // find non-zero coefficient
                    int swapRow = col + 1;
                    for (; swapRow < rowCount; swapRow++)
                        if (workingMatrix[swapRow, col] != 0)
                            break;

                    if (workingMatrix[swapRow, col] != 0) // found a non-zero coefficient?
                    {
                        // yes, then swap it with the above
                        double[] tmp = new double[rowCount + 1];
                        for (int i = 0; i < rowCount + 1; i++)
                        {
                            tmp[i] = workingMatrix[swapRow, i];
                            workingMatrix[swapRow, i] = workingMatrix[col, i];
                            workingMatrix[col, i] = tmp[i];
                        }
                    }
                    else
                        throw new ArgumentException("Set of linear equations does not have a solution");
                }

            // elimination
            for (int sourceRow = 0; sourceRow + 1 < rowCount; sourceRow++)
            {
                for (int destRow = sourceRow + 1; destRow < rowCount; destRow++)
                {
                    double df = workingMatrix[sourceRow, sourceRow];
                    double sf = workingMatrix[destRow, sourceRow];
                    for (int i = 0; i < rowCount + 1; i++)
                        workingMatrix[destRow, i] = workingMatrix[destRow, i] * df - workingMatrix[sourceRow, i] * sf;
                }
            }

            // back-insertion
            for (int row = rowCount - 1; row >= 0; row--)
            {
                double f = workingMatrix[row, row];
                if (f == 0)
                    throw new ArgumentException("Set of linear equations does not have a solution");

                for (int i = 0; i < rowCount + 1; i++)
                    workingMatrix[row, i] /= f;

                for (int destRow = 0; destRow < row; destRow++)
                {
                    workingMatrix[destRow, rowCount] -= workingMatrix[destRow, row] * workingMatrix[row, rowCount];
                    workingMatrix[destRow, row] = 0;
                }
            }

            for (int row = 0; row < rowCount; row++)
                indeterminates[row] = workingMatrix[row, rowCount];

            return indeterminates;
        }
    }

    public class DefaultValueDictionary<TKey, TValue>(TValue defaultValue) : Dictionary<TKey, TValue> where TKey : notnull
    {
        private TValue DefaultValue { get; } = defaultValue;

        public new TValue this[TKey key]
        {
            get => TryGetValue(key, out var t) ? t : DefaultValue;
            set => base[key] = value;
        }
    }

    // This is an attempt to add a Contains() method to the PriorityQueue but it doesn't seem to be
    // working correctly (as discovered in Day 17's solution). I ended up using SimplePriorityQueue
    // which I found a nuget package for. I may come back to fix this some time, but until then,
    // don't use it.
    public class CustomPriorityQueue<TElement, TPriority> : PriorityQueue<TElement, TPriority>
    {
        private List<TElement> elementList = [];

        public CustomPriorityQueue() : base()
        {
        }

        public new void Enqueue(TElement item, TPriority priority)
        {
            base.Enqueue(item, priority);
            elementList.Add(item);
        }

        public new TElement Dequeue()
        {
            var element = base.Dequeue();
            if (!elementList.Remove(element))
                throw new InvalidDataException();

            return element;
        }

        public bool Contains(TElement item)
        {
            return elementList.Contains(item);
        }
    }
}
